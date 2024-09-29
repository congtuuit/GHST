using System;

namespace GHSTShipping.Application.DTOs.User
{
    public class NoticeDto
    {
        public Guid Id { get; set; }

        public string Avatar { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Date format 2017-08-07
        /// </summary>
        public string Datetime { get; set; }

        /// <summary>
        /// Type: notification | message | event
        /// </summary>
        public string Type { get; set; }

        public bool ClickClose { get; set; }

        /// <summary>
        /// Status: urgent | doing | todo | processing
        /// </summary>
        public string Status { get; set; }

        public bool Read {  get; set; }
    }
}
