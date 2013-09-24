using System;

namespace ByteCarrot.Rulla.Tests.Rules
{
    public class TestModel
    {
        public string StringProperty { get; set; }

        public int Int32Property { get; set; }

        public DateTime DateTimeProperty { get; set; }

        public int PropertyWithSetterOnly { private get; set; }

        private int PrivateProperty { get; set; }

        protected int ProtectedProperty { get; set; }
    }
}