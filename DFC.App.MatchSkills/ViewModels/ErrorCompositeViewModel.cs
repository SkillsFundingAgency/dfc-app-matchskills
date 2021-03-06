﻿using System.Text.RegularExpressions;

namespace DFC.App.MatchSkills.ViewModels
{
    public class ErrorCompositeViewModel : CompositeViewModel
    {
        private string _rtaCode;

        public string RTACode { get { return _rtaCode; } set { _rtaCode = FormatRTACode(value); } }

        public ErrorCompositeViewModel() : base(PageId.Error, "Service Error")
        {
            _rtaCode = string.Empty;
        }

        private string FormatRTACode(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;
            var result = Regex.Replace(value.Trim().ToUpper(), ".{4}", "$0 ");
            return result;
        }
    }
}
