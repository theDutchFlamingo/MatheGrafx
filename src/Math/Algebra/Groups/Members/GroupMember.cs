using System;
using Math.Algebra.Monoids.Members;

namespace Math.Algebra.Groups.Members
{
    public abstract class GroupMember : MonoidMember, INegatable
    {
        internal abstract override T Add<T>(T other);

        public abstract override T Null<T>();
        
        /// <summary>
        /// Whether this member of the FieldMember is the null member.
        /// Is abstract because the subtype needs to test Equals(Null&lt;Type&gt;)
        /// with Type the Type corresponding to the inheriting class.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsNull();

        public abstract T Negative<T>() where T : INegatable;

        [Obsolete("Not all groups need to support a conversion to double")]
        public abstract double ToDouble();

        public abstract override bool Equals<T>(T other);

        /// <summary>
        /// Gets the double version of this object; not necessarily possible for all instances.
        /// For complex numbers, for example, only possible if the imaginary part is 0.
        /// </summary>
        /// <param name="groupMember"></param>
        public static explicit operator double (GroupMember groupMember)
        {
            return groupMember.ToDouble();
        }
    }
}