namespace System
{
    using Collections.Generic;
    using Linq;
    using Text;
    using Text.RegularExpressions;

    /// <summary>
    ///     Provides extenion methods & Functions used in Text and <see cref="String" />
    ///     manipulation.
    /// </summary>
    public static class TextExtensions
    {
        #region Declarations

        #region Static Declarations

        private static readonly char WhiteSpace = ' ';

        private static readonly Regex ReadableRegEx = new Regex(@"(\S)([A-Z]+|(\d+)(?![A-Z_\-\.]|\b|\s)|[_\-\.]+)",
            RegexOptions.Compiled);

        private static readonly Regex ReadableRemovedRegEx = new Regex(@"[_\-\.]+");

        private static readonly Regex ReadableRegEx2 = new Regex(@"(\S)([A-Z]+|(\d+)(?![A-Z_\-\.]|\b|\s)|[_\-\.]+)",
            RegexOptions.Compiled);

        private static readonly Regex ReadableRemovedRegEx2 = new Regex(@"[_\-\.]+");

        #endregion

        #endregion
        #region Methods & Functions

        public static bool IsNumber(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            double val;
            return double.TryParse(value, out val);
        }

        public static string AsReadable(this string value,
            ReadablilityCondition normalizeConditions = ReadablilityCondition.Default)
        {
            if (value.IsNullEmptyOrWhiteSpace())
                return value;

            if (normalizeConditions == 0)
                normalizeConditions = ReadablilityCondition.Default;

            string workingValue = value;

            // Validate Whitespace Trim Conditions 
            if (normalizeConditions.TrimStartWhitespace())
                workingValue = workingValue.TrimStart();

            if (normalizeConditions.TrimEndWhitespace())
                workingValue = workingValue.TrimEnd();

            // Validate Normalization Conditions 
            if (!normalizeConditions.CanMakeReadable(workingValue))
                return workingValue;

            // Declarations 
            StringBuilder returnValue = new StringBuilder();
            IEnumerable<string> workingValues = workingValue.SeperateForReadability(normalizeConditions);
            IEnumerator<string> iterator = workingValues.GetEnumerator();
            bool hasValue = iterator.MoveNext();
            bool isFirst = true;

            while (hasValue)
            {
                returnValue.Append(isFirst ? normalizeConditions.Capitalize(iterator.Current) : iterator.Current);
                hasValue = iterator.MoveNext();
                isFirst = false;
                if (hasValue)
                    returnValue.Append(WhiteSpace);
            }

            return returnValue.ToString();
        }

        private static bool InsertLeadingWhiteSpace(this char[] value, int index)
        {
            if (value.HasPrevious(index)
                && value.PreviousIsUpper(index)
                && (char.IsLower(value[index]) || char.IsUpper(value[index]) && value.NextIsLower(index)))
                return true;

            return false;
        }

        private static bool InsertTrailingWhiteSpace(this char[] value, int index)
        {
            if (value.HasNext(index)
                && value.NextIsLower(index))
                return true;

            return false;
        }

        /// <summary>
        ///     Determines if the <paramref name="value" /> is either a <c> Null </c> instance, or is an
        ///     <c> Empty </c><see cref="string" />.
        /// </summary>
        /// <param name="value"> The <see cref="String" /> to compare. </param>
        /// <returns>
        ///     <c> True </c> if the <see cref="string" /> is <c> Null </c> or <c> Empty </c>.
        /// </returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        ///     Determines if the <paramref name="value" /> is either a <c> Null </c> instance, or is an
        ///     <c> Empty </c> or <c> Whitepsace </c><see cref="string" />..
        /// </summary>
        /// <param name="value"> The <see cref="String" /> to compare. </param>
        /// <returns>
        ///     <c> True </c> if the <see cref="string" /> is <c> Null </c>, <c> Empty </c> or
        ///     <c>
        ///         Whitespace
        ///     </c>
        ///     .
        /// </returns>
        public static bool IsNullEmptyOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        private static IEnumerable<string> SeperateForReadability(this string value, ReadablilityCondition conditions)
        {
            char[] valueContent = value.ToCharArray();
            List<List<char>> vals = new List<List<char>>();
            for (int i = 0; i < valueContent.Length; i++)
            {
                if (i == 0
                    || (valueContent.IsUpper(i)
                        && (valueContent.PreviousIsLower(i)
                            || (valueContent.PreviousIsDigit(i) && conditions.HasFlag(ReadablilityCondition.ByDigit))
                            || valueContent.PreviousIsUnderscore(i) || valueContent.NextIsLower(i)))
                    || (conditions.HasFlag(ReadablilityCondition.ByDigit) && valueContent.IsDigit(i)
                        && !valueContent.PreviousIsDigit(i))
                    || (conditions.HasFlag(ReadablilityCondition.ByUnderscore) && valueContent.IsUnderscore(i)))
                    vals.Add(new List<char>());

                if (!valueContent.IsUnderscore(i))
                    vals[vals.Count - 1].Add(valueContent[i]);
            }

            foreach (List<char> i in vals)
                yield return new string(i.ToArray());
        }

        private static bool CanMakeReadable(this ReadablilityCondition conditions, string value)
        {
            if (conditions.HasFlag(ReadablilityCondition.StopIfAnyWhitespace)
                && value.Contains(WhiteSpace))
                return false;

            return true;
        }

        private static bool TrimStartWhitespace(this ReadablilityCondition conditions)
        {
            return conditions.HasFlag(ReadablilityCondition.TrimLeadingWhiteSpace);
        }

        private static bool TrimEndWhitespace(this ReadablilityCondition conditions)
        {
            return conditions.HasFlag(ReadablilityCondition.TrimTrailingWhiteSpace);
        }

        private static bool Capitalize(this ReadablilityCondition conditions)
        {
            return conditions.HasFlag(ReadablilityCondition.CapitalizeFirstCharacter);
        }

        private static string Capitalize(this ReadablilityCondition conditions, string value)
        {
            if (conditions.Capitalize()
                && !value.IsNullEmptyOrWhiteSpace())
            {
                char firstChar = value[0];
                string substring = value.Length == 0 ? string.Empty : value.Substring(1);

                if (char.IsLower(firstChar))
                    return string.Format("{0}{1}", char.ToUpper(firstChar), substring);
            }
            return value;
        }

        private static void Capitalize(this ReadablilityCondition conditions, ref string value)
        {
            value = conditions.Capitalize(value);
        }

        private static bool NextIsUpper(this char[] value, int index)
        {
            char @char;
            if (!value.HasNext(index, out @char))
                return false;

            return char.IsUpper(@char);
        }

        private static bool IsUpper(this char[] value, int index)
        {
            return char.IsUpper(value[index]);
        }

        private static bool IsDigit(this char[] value, int index)
        {
            return char.IsDigit(value[index]) || char.IsNumber(value[index]);
        }

        private static bool IsUnderscore(this char[] value, int index)
        {
            return value.IsOther(index, '_');
        }

        private static bool IsOther(this char[] value, int index, params char[] other)
        {
            return other.Contains(value[index]);
        }

        private static bool PreviousIsUpper(this char[] value, int index)
        {
            char @char;
            if (!value.HasPrevious(index, out @char))
                return false;

            return char.IsUpper(@char);
        }

        private static bool IsLower(this char[] value, int index)
        {
            return char.IsLower(value[index]);
        }

        private static bool NextIsLower(this char[] value, int index)
        {
            char @char;
            if (!value.HasNext(index, out @char))
                return false;

            return char.IsLower(@char);
        }

        private static bool PreviousIsLower(this char[] value, int index)
        {
            char @char;
            if (!value.HasPrevious(index, out @char))
                return false;

            return char.IsLower(@char);
        }

        private static bool NextIsDigit(this char[] value, int index)
        {
            char @char;
            if (!value.HasNext(index, out @char))
                return false;

            return char.IsDigit(@char) || char.IsNumber(@char);
        }

        private static bool PreviousIsDigit(this char[] value, int index)
        {
            char @char;
            if (!value.HasPrevious(index, out @char))
                return false;

            return char.IsDigit(@char) || char.IsNumber(@char);
        }

        private static bool NextIsUnderscore(this char[] value, int index)
        {
            return value.NextIsOther(index, '_');
        }

        private static bool PreviousIsUnderscore(this char[] value, int index)
        {
            return value.PreviousIsOther(index, '_');
        }

        private static bool NextIsOther(this char[] value, int index, params char[] others)
        {
            char @char;
            if (!value.HasNext(index, out @char))
                return false;

            return others.Contains(@char);
        }

        private static bool PreviousIsOther(this char[] value, int index, params char[] others)
        {
            char @char;
            if (!value.HasPrevious(index, out @char))
                return false;

            return others.Contains(@char);
        }

        private static bool HasNext(this char[] value, int index)
        {
            return (index + 1 < value.Length);
        }

        private static bool HasNext(this char[] value, int index, out char @char)
        {
            @char = default(char);
            if (!value.HasNext(index))
                return false;

            @char = value[index + 1];
            return true;
        }

        private static bool HasPrevious(this char[] value, int index)
        {
            return index > 0;
        }

        private static bool HasPrevious(this char[] value, int index, out char @char)
        {
            @char = default(char);
            if (!value.HasPrevious(index))
                return false;

            @char = value[index - 1];
            return true;
        }

        #endregion

        //    return ReadableRegEx.Replace(text, (m) => string.Format("{0} {1}", replace(m.Groups[1]), replace(m.Groups[2])));
        //{

        //public static string AsReadable2(this string text)
        //}
        //    return regex.Replace(text, (m) => string.Format("{0} {1}", replace(m.Groups[1]), replace(m.Groups[2])));
        //    Func<Group, string> replace = (g) => removed.Replace(g.Value, string.Empty);
        //    Regex removed = new Regex(@"[_\-\.]+");
        //    Regex regex = new Regex(@"(\S)([A-Z]+|(\d+)(?![A-Z_\-\.]|\b|\s)|[_\-\.]+)");
        //{

        //public static string AsReadable4(this string text)
        //}
        //    return regex.Replace(text, (m) => string.Format("{0} {1}", replace(m.Groups[1]), replace(m.Groups[2])));
        //    Func<Group, string> replace = (g) => removed.Replace(g.Value, string.Empty);
        //    Regex removed = new Regex(@"[_\-\.]+", RegexOptions.Compiled);

        //public static string AsReadable6(this string text)
        //{
        //    Regex regex = new Regex(@"(\S)([A-Z]+|(\d+)(?![A-Z_\-\.]|\b|\s)|[_\-\.]+)");
        //    List<string> result = new List<string>();

        // return regex.Replace(text, delegate(Match m) { Regex removed = new Regex(@"[_\-\.]+");
        // return removed.Replace(m.Groups[1].Value, String.Empty) + " " +
        // removed.Replace(m.Groups[2].Value, String.Empty); });

        //}

        //public static string AsReadable5(this string text)
        //{

        //    Regex regex = new Regex(@"(\S)([A-Z]+|(\d+)(?![A-Z_\-\.]|\b|\s)|[_\-\.]+)", RegexOptions.Compiled);
    }

    /// <summary>
    ///     A bitwise flag used to indicate the conditions to use when performing normalization.
    /// </summary>
    [Flags]
    public enum ReadablilityCondition : byte
    {
        /// <summary>
        ///     Specifies that Normalization should not happen if the value contains any whitespace.
        ///     <remarks>
        ///         If specified with
        ///         <value> TrimLeadingWhiteSpace </value>
        ///         or
        ///         <value>
        ///             TrimTrailingWhiteSpace
        ///         </value>
        ///         then any leading or trailing whitespace will be removed
        ///         prior to checking for this flag.
        ///     </remarks>
        /// </summary>
        StopIfAnyWhitespace = 1,

        /// <summary>
        ///     Specifies that the value to be normalized should have any leading whitespace removed,
        ///     prior to normalization.
        /// </summary>
        TrimLeadingWhiteSpace = 2,

        /// <summary>
        ///     Specifies that the value to be normalized should have any trailing whitespace removed,
        ///     prior to normalization.
        /// </summary>
        TrimTrailingWhiteSpace = 4,

        /// <summary>
        ///     Specifies that the value to be normalized should have any leading and/or trailing
        ///     whitespace removed, prior to normalization.
        /// </summary>
        TrimWhiteSpace = 6,

        /// <summary>
        ///     Specifies that normalization should be performed on whether the Character is Upper or
        ///     Lower case.
        /// </summary>
        ByCase = 8,

        /// <summary>
        ///     Specifies that normalization should be performed on whether the Character is a Digit or not.
        /// </summary>
        ByDigit = 16,

        /// <summary>
        ///     Specifies that normalization should be performed on whether the Character is an
        ///     Underscore '_' character.
        /// </summary>
        ByUnderscore = 32,

        /// <summary>
        ///     Specifies that normalization should make the first Character Upper case if it is not.
        /// </summary>
        CapitalizeFirstCharacter = 64,

        /// <summary>
        ///     Specifies the default normalization conditions.
        /// </summary>
        Default = 126
    }
}