namespace Example.WebApplication.Models
{
    using System;

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !String.IsNullOrEmpty(RequestId);
    }
}
