
namespace VolgaVM.Memory
{
    internal enum Cells { RAM, CONSOLE, ROM }
    internal class MemoryController
    {
        public MemoryCell[] cells =
        {
            (0x0000,0x4000,true), //RAM
            (0x4000,0x0010,true), //CONSOLE
            (0x4100,0x0100,true), //STACK
            (0xF000,0x1000,false) //ROM
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
