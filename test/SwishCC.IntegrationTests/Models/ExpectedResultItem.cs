// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace SwishCC.IntegrationTests.Models
{
    public class ExpectedResultItem
    {
        [JsonPropertyName("return_code")]
        public int ReturnCode { get; set; }

        [JsonPropertyName("stdout")]
        public string StandardOutput { get; set; }
    }
}
