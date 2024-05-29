﻿namespace Domain.Utilities
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public Response(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public Response(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
