using System;
using System.IO;
using VolgaVM;
namespace VolgaVM.Bootloader{
    class Program{
        public static void Main(string[] args){
            byte[] rom;
            string name = "bin.bin";
            if(args.Length >= 1){
                name = args[0];
            }
            using(BinaryReader br = new(File.OpenRead(name))){
                rom = br.ReadBytes(0x7000);
            }
            Machine.Run(rom);
        }
    }
}