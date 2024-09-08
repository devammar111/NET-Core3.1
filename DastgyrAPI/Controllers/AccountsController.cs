using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using DastgyrAPI.Common;
using DastgyrAPI.Service;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using DastgyrAPI.Models.ViewModels.Responses;
using System.Net;
using System.Threading.Tasks;
using DastgyrAPI.Models.ViewModels.RequestModels;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;

namespace DastgyrAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class AccountsController : ControllerBase
    {
        private IUserService _userService;
        private readonly SecretKeys _appKeys;
        HttpClient client = new HttpClient();

        public AccountsController(IUserService userService,
            IOptions<SecretKeys> appKeys
            )
        {
            _appKeys = appKeys.Value;
            _userService = userService;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                string bsmSitsKey = _appKeys.BsmsitsKey;
                string fromNumber = _appKeys.FromNumber;
                string twilioAccountSid = _appKeys.TwilioAccountSid;
                string twilioAuthToken = _appKeys.TwilioAuthToken;

                if (string.IsNullOrWhiteSpace(loginRequest.Password))
                {

                    if (!await _userService.GetUserByMobile(loginRequest.UserName))
                    {
                        return BadRequest(new { message = "Username is incorrect" });
                    }
                    var code = new Random().Next(1000, 9999).ToString();
                    if (loginRequest.IsTwilio == true)
                    {
                        // Find your Account SID and Auth Token at twilio.com/console
                        // and set the environment variables. See http://twil.io/secure
                        string accountSid = twilioAccountSid;
                        string authToken = twilioAuthToken;

                        TwilioClient.Init(accountSid, authToken);

                        var message = MessageResource.Create(
                            body: "Your OTP is " + code + " please do not share it with anyone.\nDastgyr\nTo5KgAjxBxy",
                            from: fromNumber,
                            to: new Twilio.Types.PhoneNumber(loginRequest.UserName)
                        );
                    }
                    else {
                        string accountSid = bsmSitsKey;
                        HttpResponseMessage response = await client.GetAsync(
                            "http://bsms.its.com.pk/otpsms.php?key=" + accountSid + "&receiver=" + loginRequest.UserName + "& sender=DASTGYR&OtpCode=" + code);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var otpResponse = JsonConvert.DeserializeObject<OTPSentResponse>(responseBody);
                        if (otpResponse.Response.Status.Equals("Error"))
                        {
                            return Ok(new OTPResponse() { Status = otpResponse.Response.ErrorNo, Message = otpResponse.Response.Description });
                        }
                    }
                    
                    
                    var result = await _userService.CreateMobileVerificationRecord(loginRequest.UserName, code);
                    if (result > 0)
                    {
                        return Ok(new OTPResponse() { Status = "200", Message = "OTP has sent to your number" });

                    }


                }
                else
                {
                    // get the user to verifty
                    var response = await _userService.Authenticate(loginRequest);

                    if (response == null)
                        return BadRequest(new { message = "Username or password is incorrect" });

                    return Ok(response);
                }

            }
            var errorList = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.FirstOrDefault().ErrorMessage
            );
            return BadRequest(new ErrorResponse() { Code = "400", Errors = errorList.Select(e => new Error { Title = e.Key, Description = e.Value }).ToList() });
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> VerifyOTP([FromBody] LoginRequestOTP loginRequest)
        {
            if (ModelState.IsValid)
            {
                // get the user to verifty
                var response = await _userService.AuthenticateWithOTP(loginRequest.UserName, loginRequest.OTP);

                if (response == null)
                    return BadRequest(new { message = "Invalid OTP" });

                return Ok(response);


            }
            var errorList = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.FirstOrDefault().ErrorMessage
            );
            return BadRequest(new ErrorResponse() { Code = "400", Errors = errorList.Select(e => new Error { Title = e.Key, Description = e.Value }).ToList() });
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Encryption([FromBody] string password)
        {
            if (ModelState.IsValid)
            {

                var key = new byte[] { 7, 2, 8, 1, 5, 3, 5, 1, 5, 6, 4, 9, 2, 3, 1, 6 };
                var encrypted = EncryptStringToBytes_Aes(password, key);
                //byte[] bytes = new byte[1024 * 1024];
                var decrypt = Decrypt("60BF1D26C0542C7880916EE7D6D33AF1", "7281535156492316");
                var res = Encrypt("123456", "7281535156492316");
                string result1 = BitConverter.ToString(encrypted).Replace("-", string.Empty);
                string result2 = Convert.ToBase64String(encrypted);
                string result3 = Encoding.UTF8.GetString(encrypted) ;

                return Ok(res);


            }
            var errorList = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.FirstOrDefault().ErrorMessage
            );
            return BadRequest(new ErrorResponse() { Code = "400", Errors = errorList.Select(e => new Error { Title = e.Key, Description = e.Value }).ToList() });
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CheckUserWithRole([FromBody] LoginRequestOTP loginRequest)
        {
            if (ModelState.IsValid)
            {
                // get the user to verifty
                var response = await _userService.CheckUserWithRole(loginRequest.UserName);

                if (response == null)
                    return BadRequest(new { message = "Invalid OTP" });

                return Ok(response);


            }
            var errorList = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.FirstOrDefault().ErrorMessage
            );
            return BadRequest(new ErrorResponse() { Code = "400", Errors = errorList.Select(e => new Error { Title = e.Key, Description = e.Value }).ToList() });
        }


        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OTPResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddFcmToken(string fcmToken)
        {
            if (ModelState.IsValid)
            {
                //get the user to verifty
                var response = await _userService.AddFcmToken(fcmToken);
                if (response)
                    return Ok(new OTPResponse() { Status = "200", Message = "FCM token added." });
                else
                    return BadRequest(new { message = "there was a problem in adding FCM token." });

            }
            var errorList = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.FirstOrDefault().ErrorMessage
            );
            return BadRequest(new ErrorResponse() { Code = "400", Errors = errorList.Select(e => new Error { Title = e.Key, Description = e.Value }).ToList() });
        }

        byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key)
        {
            byte[] encrypted;
            byte[] IV;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;

                aesAlg.GenerateIV();
                IV = aesAlg.IV;

                aesAlg.Mode = CipherMode.CBC;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            var combinedIvCt = new byte[IV.Length + encrypted.Length];
            Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
            Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);

            // Return the encrypted bytes from the memory stream. 
            return combinedIvCt;

        }
         string Encrypt(string input, string key)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncrptArray = UTF8Encoding.UTF8.GetBytes(input);
            Aes kgen = Aes.Create("AES");
            kgen.Mode = CipherMode.ECB;
            kgen.Key = keyArray;
            ICryptoTransform cTransform = kgen.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncrptArray, 0, toEncrptArray.Length);
            return BitConverter.ToString(resultArray).Replace("-", "");
            //return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        string Decrypt(string input, string key)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncrptArray = UTF8Encoding.UTF8.GetBytes(input);
            Aes kgen = Aes.Create("AES");
            kgen.Mode = CipherMode.ECB;
            kgen.Key = keyArray;
            ICryptoTransform cTransform = kgen.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncrptArray, 0, toEncrptArray.Length);
            return BitConverter.ToString(resultArray).Replace("-", "");
            //return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }


    }
}