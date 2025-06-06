﻿using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
namespace App.Common.Helper
{
    /// <summary> CÁC core xử lý liên quan đến string </summary>
    /// Author: thuanbv
    /// Created: 23/04/2025
    /// Modified: date - user - description
    public class StringHepler
    {
        /// <summary>Bỏ dấu tiếng Việt và chuyển thành chữ in hoa</summary>
        /// <param name="text">Chuỗi cần xử lý</param>
        /// <returns>Chuỗi đã được bỏ dấu và chuyển thành in hoa</returns>
        /// Author: thuanbv
        /// Created: 29/04/2025
        /// Modified: date - user - description
        public static string RemoveDiacriticsToUpper(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                                  "đ",
                                  "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                                  "í","ì","ỉ","ĩ","ị",
                                  "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                                  "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                                  "ý","ỳ","ỷ","ỹ","ỵ",};

            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                                  "d",
                                  "e","e","e","e","e","e","e","e","e","e","e",
                                  "i","i","i","i","i",
                                  "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                                  "u","u","u","u","u","u","u","u","u","u","u",
                                  "y","y","y","y","y",};

            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }

            return text.ToUpper();
        }


    }
}