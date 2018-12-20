using System;
using System.Collections.Generic;
using System.Numerics;

namespace AdventOfCode
{
    class Register
    {
        Dictionary<string, RegisterValue> registers = new Dictionary<string, RegisterValue>();
        BigInteger lastSound = 0;


        public void Snd(string reg)
        {
            lastSound = registers[reg].currentValue;
        }

        public void Set(string reg, int val)
        {
            checkIfRegisterExists(reg);
            registers[reg].currentValue = val;
        }

        private void checkIfRegisterExists(string reg)
        {
            if (!registers.ContainsKey(reg))
            {
                registers[reg] = new RegisterValue();
            }
        }

        public void Add(string reg, BigInteger val)
        {
            checkIfRegisterExists(reg);
            registers[reg].currentValue += val;
        }

        public void Mul(string reg, BigInteger multiplicator)
        {
            checkIfRegisterExists(reg);
            registers[reg].currentValue *= multiplicator;
        }

        public void Mod(string reg, BigInteger dividor)
        {
            checkIfRegisterExists(reg);
            registers[reg].currentValue %= dividor;
        }

        public BigInteger Rcv(string reg)
        {
            checkIfRegisterExists(reg);
            if (registers[reg].currentValue != 0)
            {
                registers[reg].currentValue = lastSound;
                Console.WriteLine(lastSound);
                Console.ReadLine();
                Snd(reg);
            }
            return int.MaxValue;
        }

        public BigInteger Jgz(string reg, BigInteger offset)
        {
            checkIfRegisterExists(reg);
            if (registers[reg].currentValue > 0)
            {
                return offset;
            }
            return int.MaxValue;
        }

        public class RegisterValue
        {
            public BigInteger currentValue = 0;
        }

    }
}
