using System;

namespace Acklann.Diffa.Exceptions
{
    /// <summary>
    /// Represents errors that occur when a file do not match it's approved file.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ResultNotApprovedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultNotApprovedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ResultNotApprovedException(string message) : base(message)
        {
            HelpLink = ExceptionMessage.IssuesLink;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultNotApprovedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public ResultNotApprovedException(string message, Exception inner) : base(message, inner)
        {
            HelpLink = ExceptionMessage.IssuesLink;
        }
    }
}