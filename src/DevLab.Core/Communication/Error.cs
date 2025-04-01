namespace DevLab.Core.Communication;
    
    /// <summary>
    /// Represents an application error. It may or may not contain a code.
    /// </summary>
    /// <param name="Message">The error message.</param>
    /// <param name="Code">The error code (optional).</param>
    public sealed record Error(string Message, string? Code = null)
    {
        /// <summary>
        /// Represents the absence of an error.
        /// </summary>
        public static readonly Error None = new("None", "No error");
    
        /// <summary>
        /// Represents a null value error.
        /// </summary>
        public static readonly Error NullValue = new("NullValue", "Value is null");
    
        /// <summary>
        /// Returns the string representation of the error.
        /// </summary>
        /// <returns>A string that represents the error, including the code if available.</returns>
        public override string ToString()
        {
            return !string.IsNullOrEmpty(Code) ? $"{Code}: {Message}" : Message;
        }
    }