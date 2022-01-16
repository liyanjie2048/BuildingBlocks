using System.Collections.Generic;

namespace Liyanjie.ValueObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class Name : ValueObject
    {
        public string GivenName { get; set; }

        public string Surname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return GivenName;
            yield return Surname;
        }
    }
}
