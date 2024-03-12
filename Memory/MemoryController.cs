
namespace VolgaVM.Memory
{
    internal enum Cells { RAM, CONSOLE, ROM }
    internal class MemoryController
    {
        public MemoryCell[] cells =
        {
            (0x0000,0x8000,true), //RAM
            (0x8000,0x0100,true), //STACK
            (0x8100,0x0010,true), //REGS
            (0x9000,0x7000,false) //ROM
        };
        readonly private byte[] memory = new byte[ushort.MaxValue];
        public byte? this[ushort address]
        {
            get
            {
                foreach(var cell in cells)
                {
                    if(address >= cell.Start && address <= cell.Start + cell.Length)
                    {
                        return memory[address];
                    }
                }
                return null;
            }
            set
            {
                foreach (var cell in cells)
                {
                    if (address >= cell.Start && address <= cell.Start + cell.Length && cell.Writable)
                    {
                        memory[address] = value ?? 0;
                    }
                }
            }
        }
    }
}
