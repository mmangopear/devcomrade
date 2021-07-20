﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppLogic.Helpers
{
    internal static class ClipboardFormats
    {
        static readonly string HEADER = 
            "Version:0.9\r\n" +
            "StartHTML:{0:0000000000}\r\n" +
            "EndHTML:{1:0000000000}\r\n" +
            "StartFragment:{2:0000000000}\r\n" +
            "EndFragment:{3:0000000000}\r\n";

        static readonly string HTML_START =
            "<html>\r\n" +
            "<body>\r\n" +
            "<!--StartFragment-->";

        static readonly string HTML_END =
            "<!--EndFragment-->\r\n" +
            "</body>\r\n" +
            "</html>";

        public static string ConvertHtmlToClipboardData(string html)
        {
            var encoding = new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
            var data = Array.Empty<byte>();

            var header = encoding.GetBytes(String.Format(HEADER, 0, 1, 2, 3));
            data = data.Concat(header).ToArray();

            var startHtml = data.Length;
            data = data.Concat(encoding.GetBytes(HTML_START)).ToArray();

            var startFragment = data.Length;
            data = data.Concat(encoding.GetBytes(html)).ToArray();

            var endFragment = data.Length;
            data = data.Concat(encoding.GetBytes(HTML_END)).ToArray();

            var endHtml = data.Length;

            var newHeader = encoding.GetBytes(
                String.Format(HEADER, startHtml, startFragment, endFragment, endHtml));
            if (newHeader.Length != startHtml)
            {
                throw new InvalidOperationException(nameof(ConvertHtmlToClipboardData));
            }

            Array.Copy(newHeader, data, length: startHtml);
            return encoding.GetString(data);
        } 
    }
}
