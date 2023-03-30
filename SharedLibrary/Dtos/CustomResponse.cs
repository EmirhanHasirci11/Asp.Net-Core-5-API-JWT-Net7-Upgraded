using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
    public class CustomResponse<T> where T : class
    {
        public T Data { get; private set; }
        public int StatusCode { get; private set; }
        public ErrorDto Error { get;private set; }
        [JsonIgnore]
        public bool IsSuccessful { get;private set; }
        public static CustomResponse<T> Success(T data, int statusCode)
        {
            return new CustomResponse<T>() { Data = data, StatusCode = statusCode,IsSuccessful=true };
        }
        public static CustomResponse<T> Success(int statusCode)
        {
            return new CustomResponse<T>() { Data = default, StatusCode = statusCode,IsSuccessful = true };
        }
        public static CustomResponse<T> Fail(ErrorDto errorDto, int statusCode)
        {
            return new CustomResponse<T>() { Error = errorDto, StatusCode = statusCode,IsSuccessful=false };
        }
        public static CustomResponse<T> Fail(string errorMessage, int statusCode, bool isShow)
        {
            var errorDto = new ErrorDto(errorMessage, isShow);
            return new CustomResponse<T>() { Error = errorDto, StatusCode = statusCode,IsSuccessful=false };
        }

        
    }
}
