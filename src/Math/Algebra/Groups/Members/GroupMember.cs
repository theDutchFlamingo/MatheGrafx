using Math.Algebra.Monoids.Members;

namespace Math.Algebra.Groups.Members
{
    public abstract class GroupMember : MonoidMember
    {
        internal abstract T Multiply<T>(T other) where T : GroupMember;

        public abstract T Null<T>() where T : GroupMember;

        public abstract T Unit<T>() where T : GroupMember;

        public abstract double ToDouble();

        /// <summary>
        /// Whether this member of the FieldMember is the null member.
        /// Is abstract because the subtype needs to test Equals(Null&lt;Type&gt;)
        /// with Type the Type corresponding to the inheriting class.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsNull();

        /// <summary>
        /// Whether this member of the FieldMember is the unit member.
        /// Is abstract because the subtype needs to test Equals(Null&lt;Type&gt;)
        /// with Type the Type corresponding to the inheriting class.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsUnit();

        public abstract bool Equals<T>(T other);

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