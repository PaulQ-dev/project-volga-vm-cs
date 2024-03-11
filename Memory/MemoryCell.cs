namespace VolgaVM.Memory
{
    internal readonly record struct MemoryCell(ushort Start, ushort Length, bool Writable)
    {
        public static implicit operator (ushort s, ushort l, bool w)(MemoryCell value)
        {
            return (value.Start, value.Length, value.Writable);
        }

        public static implicit operator MemoryCell((ushort s, ushort l, bool w) value)
        {
            return new MemoryCell(value.s, value.l, value.w);
        }
    }
}
