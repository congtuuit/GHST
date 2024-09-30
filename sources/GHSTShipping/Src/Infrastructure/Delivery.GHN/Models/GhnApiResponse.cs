﻿using Newtonsoft.Json;

namespace Delivery.GHN.Models
{
    public class GhnApiResponse<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("message_display")]
        public string MessageDisplay { get; set; }

        public GhnApiResponse(int code, T data, string message)
        {
            Code = code;
            Data = data;
            Message = message;
        }
    }
}
