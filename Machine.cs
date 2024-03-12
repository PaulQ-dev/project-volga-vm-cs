using System;
using VolgaVM.Memory;

namespace VolgaVM
{
    public class Machine
    {
        private ushort pc, ab = 0x0000; public static readonly ushort cs = 0x8100, ci = 0x8101, co = 0x8102, cc = 0x8103, sb = 0x8000; //Program Counter, Address Buffer, Console State, Console Input, Console Output, Console Color, Stack Base
        private byte rb = 0x00, a = 0x00, x = 0x00, y = 0x00, sp = 0x00; //Read Buffer, A-reg, X-reg, Y-reg, Stack Pointer
        private readonly MemoryController mc;

        private byte RunMachine()
        {
            while (true)
            {
                byte exit_code = 0;
                bool run = true;
                ReadValue();
                switch (rb)
                {
                    //HLT
                    case 0x00:
                        run = false;
                        goto exit;
                    //HLT #
                    case 0x01:
                        run = false;
                        ReadValue();
                        exit_code = rb;
                        goto exit;
                    //HLT a
                    case 0x02:
                        run = false;
                        ReadAddress();
                        ReadValue(true);
                        exit_code = rb;
                        goto exit;
                    //STM a #
                    case 0x88: 
                        ReadAddress();
                        ReadValue();
                        mc[ab] = rb;
                        break;
                    //STA a
                    case 0x89: 
                        ReadAddress();
                        mc[ab] = a;
                        break;
                    //STX a
                    case 0x8A: 
                        ReadAddress();
                        mc[ab] = x;
                        break;
                    //STY a
                    case 0x8B: 
                        ReadAddress();
                        mc[ab] = y;
                        break;
                    //STM (a) #
                    case 0x98:
                        ReadAddress();
                        ReadAddress(true);
                        ReadValue();
                        mc[ab] = rb;
                        break;
                    //LDA #
                    case 0xA1:
                        ReadValue();
                        a = rb;
                        break;
                    //LDX #
                    case 0xA2:
                        ReadValue();
                        x = rb;
                        break;
                    //LDY #
                    case 0xA3:
                        ReadValue();
                        y = rb;
                        break;
                    //LDA a
                    case 0xA9:
                        ReadAddress();
                        ReadValue(true);
                        a = rb;
                        break;
                    //LDX a
                    case 0xAA:
                        ReadAddress();
                        ReadValue(true);
                        x = rb;
                        break;
                    //LDY a
                    case 0xAB:
                        ReadAddress();
                        ReadValue(true);
                        y = rb;
                        break;
                }
                pc++;
            exit:
                if (!run) return exit_code;
            }
        }
        /// <summary>
        /// Reads byte from machine memory and writes it to read buffer
        /// </summary>
        /// <param name="useBuffer">false - program counter is address, true - address buffer</param>
        private void ReadValue(bool useBuffer = false)
        {
            if (useBuffer) rb = mc[ab] ?? rb;
            else pc++; rb = mc[pc] ?? rb;
        }
        /// <summary>
        /// Reads two bytes from machine memory and writes it to address buffer
        /// </summary>
        private void ReadAddress(bool useBuffer = false)
        {
            if (useBuffer){
                rb = mc[ab] ?? rb;
                PushStack(rb);
                ab++;
                rb = mc[ab] ?? rb;
                ab = (ushort)(PullStack() + rb * 0x0100);
            }
            else{
                pc++; rb = mc[pc] ?? rb;
                ab = rb;
                pc++; rb = mc[pc] ?? rb;
                ab += (ushort)(rb * 0x0100);
            }
        }
        private void PushStack(byte value){
            sp++;
            mc[(ushort)(sb+sp)] = value;
        }
        private byte PullStack(){
            sp--;
            return mc[(ushort)(sb+sp+1)] ?? rb;
        }

        public Machine(byte[] rom)
        {
            mc = new();
            if (rom.Length > mc.cells[(int)Cells.ROM].Length)
            {
                throw new OverflowException();
            }
            for (ushort i = 0; i < rom.Length; i++)
            {
                mc[(ushort)(i + mc.cells[(int)Cells.ROM].Start)] = rom[i];
            }
            pc = (ushort)((mc[0x9000] ?? rb) + 0x0100 * (mc[0x9001] ?? rb));
        }

        public static void Run(byte[] rom)
        {
            System.Console.WriteLine(new Machine(rom).RunMachine());
        }
    }
}
