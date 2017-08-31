﻿using System;
using System.Linq.Expressions;

namespace One_X {
    class InstructionAttribute : System.Attribute {
        public string Name;
        public int Bytes;
        public int MCycles;
        public int TStates;
        public Action Operation;

        public InstructionAttribute(string Name, int Bytes, int MCycles, int TStates, string method) {
            this.Name = Name;
            this.Bytes = Bytes;
            this.MCycles = MCycles;
            this.TStates = TStates;
        }

        public enum OPCODE : byte {
            [Instruction("NOP", 1, 1, 4, "")] NOP = 0x00,
            [Instruction("LXI B", 3, 3, 10, "")] LXI_B = 0x01,
            [Instruction("STAX B", 1, 2, 7, "")] STAX_B = 0x02,
            [Instruction("INX B", 1, 1, 6, "")] INX_B = 0x03,
            [Instruction("INX B", 1, 1, 4, "")] INR_B = 0x04,
            [Instruction("DCR B", 1, 1, 4, "")] DCR_B = 0x05,
            [Instruction("MVI B", 2, 2, 7, "")] MVI_B = 0x06,
            [Instruction("RLC", 1, 1, 4, "")] RLC = 0x07,
            [Instruction("", 0, 0, 0, "")] UNKN_08 = 0x08,
            [Instruction("DAD B", 1, 3, 10, "")] DAD_B = 0x09,
            [Instruction("LDAX B", 1, 2, 7, "")] LDAX_B = 0x0A,
            [Instruction("DCX B", 1, 1, 6, "")] DCX_B = 0x0B,
            [Instruction("INR C", 1, 1, 4, "")] INR_C = 0x0C,
            [Instruction("DCR C", 1, 1, 4, "")] DCR_C = 0x0D,
            [Instruction("MVI C", 2, 2, 7, "")] MVI_C = 0x0E,
            [Instruction("RRC", 1, 1, 4, "")] RRC = 0x0F,
            [Instruction("", 0, 0, 0, "")] UNKN_10 = 0x10,
            [Instruction("LXI D", 3, 3, 10, "")] LXI_D = 0x11,
            [Instruction("STAX D", 1, 2, 7, "")] STAX_D = 0x12,
            [Instruction("INX D", 1, 1, 6, "")] INX_D = 0x13,
            [Instruction("INR D", 1, 1, 4, "")] INR_D = 0x14,
            [Instruction("DCR D", 1, 1, 4, "")] DCR_D = 0x15,
            [Instruction("MVI D", 2, 2, 7, "")] MVI_D = 0x16,
            [Instruction("RAL", 1, 1, 4, "")] RAL = 0x17,
            [Instruction("", 0, 0, 0, "")] UNKN_24 = 0x18,
            [Instruction("DAD D", 1, 3, 10, "")] DAD_D = 0x19,
            [Instruction("LDAX D", 1, 2, 7, "")] LDAX_D = 0x1A,
            [Instruction("DCX D", 1, 1, 6, "")] DCX_D = 0x1B,
            [Instruction("INR E", 1, 1, 4, "")] INR_E = 0x1C,
            [Instruction("DCR E", 1, 1, 4, "")] DCR_E = 0x1D,
            [Instruction("MVI E", 2, 2, 7, "")] MVI_E = 0x1E,
            [Instruction("RAR", 1, 1, 4, "")] RAR = 0x1F,
            [Instruction("RIM", 1, 1, 4, "")] RIM = 0x20,
            [Instruction("LXI H", 3, 3, 10, "")] LXI_H = 0x21,
            [Instruction("SHLD", 3, 5, 16, "")] SHLD = 0x22,
            [Instruction("INX H", 1, 1, 6, "")] INX_H = 0x23,
            [Instruction("INR H", 1, 1, 4, "")] INR_H = 0x24,
            [Instruction("DCR H", 1, 1, 4, "")] DCR_H = 0x25,
            [Instruction("MVI H", 2, 2, 7, "")] MVI_H = 0x26,
            [Instruction("DAA", 1, 1, 4, "")] DAA = 0x27,
            [Instruction("", 0, 0, 0, "")] UNKN_28 = 0x28,
            [Instruction("DAD H", 1, 3, 10, "")] DAD_H = 0x29,
            [Instruction("LHLD", 3, 5, 16, "")] LHLD = 0x2A,
            [Instruction("DCX H", 1, 1, 6, "")] DCX_H = 0x2B,
            [Instruction("INR L", 1, 1, 4, "")] INR_L = 0x2C,
            [Instruction("DCR L", 1, 1, 4, "")] DCR_L = 0x2D,
            [Instruction("MVI L", 2, 2, 7, "")] MVI_L = 0x2E,
            [Instruction("CMA", 1, 1, 4, "")] CMA = 0x2F,
            [Instruction("SIM", 1, 1, 4, "")] SIM = 0x30,
            [Instruction("LXI SP", 3, 3, 10, "")] LXI_SP = 0x31,
            [Instruction("STA", 3, 4, 13, "")] STA = 0x32,
            [Instruction("INX SP", 1, 1, 6, "")] INX_SP = 0x33,
            [Instruction("INR M", 1, 3, 10, "")] INR_M = 0x34,
            [Instruction("DCR M", 1, 3, 10, "")] DCR_M = 0x35,
            [Instruction("MVI M", 2, 3, 10, "")] MVI_M = 0x36,
            [Instruction("STC", 1, 1, 4, "")] STC = 0x37,
            [Instruction("", 0, 0, 0, "")] UNKN_38 = 0x38,
            [Instruction("DAD SP", 1, 3, 10, "")] DAD_SP = 0x39,
            [Instruction("LDA", 3, 4, 13, "")] LDA = 0x3A,
            [Instruction("DCX SP", 1, 1, 6, "")] DCX_SP = 0x3B,
            [Instruction("INR A", 1, 1, 4, "")] INR_A = 0x3C,
            [Instruction("DCR A", 1, 1, 4, "")] DCR_A = 0x3D,
            [Instruction("MVI A", 2, 2, 7, "")] MVI_A = 0x3E,
            [Instruction("CMC", 1, 1, 4, "")] CMC = 0x3F,
            [Instruction("MOV B,B", 1, 1, 4, "")] MOV_BB = 0x40,
            [Instruction("MOV B,C", 1, 1, 4, "")] MOV_BC = 0x41,
            [Instruction("MOV B,D", 1, 1, 4, "")] MOV_BD = 0x42,
            [Instruction("MOV B,E", 1, 1, 4, "")] MOV_BE = 0x43,
            [Instruction("MOV B,H", 1, 1, 4, "")] MOV_BH = 0x44,
            [Instruction("MOV B,L", 1, 1, 4, "")] MOV_BL = 0x45,
            [Instruction("MOV B,M", 1, 2, 7, "")] MOV_BM = 0x46,
            [Instruction("MOV B,A", 1, 1, 4, "")] MOV_BA = 0x47,
            [Instruction("MOV C,B", 1, 1, 4, "")] MOV_CB = 0x48,
            [Instruction("MOV C,C", 1, 1, 4, "")] MOV_CC = 0x49,
            [Instruction("MOV C,D", 1, 1, 4, "")] MOV_CD = 0x4A,
            [Instruction("MOV C,E", 1, 1, 4, "")] MOV_CE = 0x4B,
            [Instruction("MOV C,H", 1, 1, 4, "")] MOV_CH = 0x4C,
            [Instruction("MOV C,L", 1, 1, 4, "")] MOV_CL = 0x4D,
            [Instruction("MOV C,M", 1, 2, 7, "")] MOV_CM = 0x4E,
            [Instruction("MOV C,A", 1, 1, 4, "")] MOV_CA = 0x4F,
            [Instruction("MOV D,B", 1, 1, 4, "")] MOV_DB = 0x50,
            [Instruction("MOV D,C", 1, 1, 4, "")] MOV_DC = 0x51,
            [Instruction("MOV D,D", 1, 1, 4, "")] MOV_DD = 0x52,
            [Instruction("MOV D,E", 1, 1, 4, "")] MOV_DE = 0x53,
            [Instruction("MOV D,H", 1, 1, 4, "")] MOV_DH = 0x54,
            [Instruction("MOV D,L", 1, 1, 4, "")] MOV_DL = 0x55,
            [Instruction("MOV D,M", 1, 2, 7, "")] MOV_DM = 0x56,
            [Instruction("MOV D,A", 1, 1, 4, "")] MOV_DA = 0x57,
            [Instruction("MOV E,B", 1, 1, 4, "")] MOV_EB = 0x58,
            [Instruction("MOV E,C", 1, 1, 4, "")] MOV_EC = 0x59,
            [Instruction("MOV E,D", 1, 1, 4, "")] MOV_ED = 0x5A,
            [Instruction("MOV E,E", 1, 1, 4, "")] MOV_EE = 0x5B,
            [Instruction("MOV E,H", 1, 1, 4, "")] MOV_EH = 0x5C,
            [Instruction("MOV E,L", 1, 1, 4, "")] MOV_EL = 0x5D,
            [Instruction("MOV E,M", 1, 2, 7, "")] MOV_EM = 0x5E,
            [Instruction("MOV E,A", 1, 1, 4, "")] MOV_EA = 0x5F,
            [Instruction("MOV H,B", 1, 1, 4, "")] MOV_HB = 0x60,
            [Instruction("MOV H,C", 1, 1, 4, "")] MOV_HC = 0x61,
            [Instruction("MOV H,D", 1, 1, 4, "")] MOV_HD = 0x62,
            [Instruction("MOV H,E", 1, 1, 4, "")] MOV_HE = 0x63,
            [Instruction("MOV H,H", 1, 1, 4, "")] MOV_HH = 0x64,
            [Instruction("MOV H,L", 1, 1, 4, "")] MOV_HL = 0x65,
            [Instruction("MOV H,M", 1, 2, 7, "")] MOV_HM = 0x66,
            [Instruction("MOV H,A", 1, 1, 4, "")] MOV_HA = 0x67,
            [Instruction("MOV L,B", 1, 1, 4, "")] MOV_LB = 0x68,
            [Instruction("MOV L,C", 1, 1, 4, "")] MOV_LC = 0x69,
            [Instruction("MOV L,D", 1, 1, 4, "")] MOV_LD = 0x6A,
            [Instruction("MOV L,E", 1, 1, 4, "")] MOV_LE = 0x6B,
            [Instruction("MOV L,H", 1, 1, 4, "")] MOV_LH = 0x6C,
            [Instruction("MOV L,L", 1, 1, 4, "")] MOV_LL = 0x6D,
            [Instruction("MOV L,M", 1, 2, 7, "")] MOV_LM = 0x6E,
            [Instruction("MOV L,A", 1, 1, 4, "")] MOV_LA = 0x6F,
            [Instruction("MOV M,B", 1, 2, 7, "")] MOV_MB = 0x70,
            [Instruction("MOV M,C", 1, 2, 7, "")] MOV_MC = 0x71,
            [Instruction("MOV M,D", 1, 2, 7, "")] MOV_MD = 0x72,
            [Instruction("MOV M,E", 1, 2, 7, "")] MOV_ME = 0x73,
            [Instruction("MOV M,H", 1, 2, 7, "")] MOV_MH = 0x74,
            [Instruction("MOV M,L", 1, 2, 7, "")] MOV_ML = 0x75,
            [Instruction("HLT", 1, 2, 7, "")] HLT = 0x76,
            [Instruction("MOV M,A", 1, 2, 7, "")] MOV_MA = 0x77,
            [Instruction("MOV A,B", 1, 1, 4, "")] MOV_AB = 0x78,
            [Instruction("MOV A,C", 1, 1, 4, "")] MOV_AC = 0x79,
            [Instruction("MOV A,D", 1, 1, 4, "")] MOV_AD = 0x7A,
            [Instruction("MOV A,E", 1, 1, 4, "")] MOV_AE = 0x7B,
            [Instruction("MOV A,H", 1, 1, 4, "")] MOV_AH = 0x7C,
            [Instruction("MOV A,L", 1, 1, 4, "")] MOV_AL = 0x7D,
            [Instruction("MOV A,M", 1, 2, 7, "")] MOV_AM = 0x7E,
            [Instruction("MOV A,A", 1, 1, 4, "")] MOV_AA = 0x7F,
            [Instruction("ADD B", 1, 1, 4, "")] ADD_B = 0x80,
            [Instruction("ADD C", 1, 1, 4, "")] ADD_C = 0x81,
            [Instruction("ADD D", 1, 1, 4, "")] ADD_D = 0x82,
            [Instruction("ADD E", 1, 1, 4, "")] ADD_E = 0x83,
            [Instruction("ADD H", 1, 1, 4, "")] ADD_H = 0x84,
            [Instruction("ADD L", 1, 1, 4, "")] ADD_L = 0x85,
            [Instruction("ADD M", 1, 2, 7, "")] ADD_M = 0x86,
            [Instruction("ADD A", 1, 1, 4, "")] ADD_A = 0x87,
            [Instruction("ADC B", 1, 1, 4, "")] ADC_B = 0x88,
            [Instruction("ADC C", 1, 1, 4, "")] ADC_C = 0x89,
            [Instruction("ADC D", 1, 1, 4, "")] ADC_D = 0x8A,
            [Instruction("ADC E", 1, 1, 4, "")] ADC_E = 0x8B,
            [Instruction("ADC H", 1, 1, 4, "")] ADC_H = 0x8C,
            [Instruction("ADC L", 1, 1, 4, "")] ADC_L = 0x8D,
            [Instruction("ADC M", 1, 2, 7, "")] ADC_M = 0x8E,
            [Instruction("ADC A", 1, 1, 4, "")] ADC_A = 0x8F,
            [Instruction("SUB B", 1, 1, 4, "")] SUB_B = 0x90,
            [Instruction("SUB C", 1, 1, 4, "")] SUB_C = 0x91,
            [Instruction("SUB D", 1, 1, 4, "")] SUB_D = 0x92,
            [Instruction("SUB E", 1, 1, 4, "")] SUB_E = 0x93,
            [Instruction("SUB H", 1, 1, 4, "")] SUB_H = 0x94,
            [Instruction("SUB L", 1, 1, 4, "")] SUB_L = 0x95,
            [Instruction("SUB M", 1, 2, 7, "")] SUB_M = 0x96,
            [Instruction("SUB A", 1, 1, 4, "")] SUB_A = 0x97,
            [Instruction("SBB B", 1, 1, 4, "")] SBB_B = 0x98,
            [Instruction("SBB C", 1, 1, 4, "")] SBB_C = 0x99,
            [Instruction("SBB D", 1, 1, 4, "")] SBB_D = 0x9A,
            [Instruction("SBB E", 1, 1, 4, "")] SBB_E = 0x9B,
            [Instruction("SBB H", 1, 1, 4, "")] SBB_H = 0x9C,
            [Instruction("SBB L", 1, 1, 4, "")] SBB_L = 0x9D,
            [Instruction("SBB M", 1, 2, 7, "")] SBB_M = 0x9E,
            [Instruction("SBB A", 1, 1, 4, "")] SBB_A = 0x9F,
            [Instruction("ANA B", 1, 1, 4, "")] ANA_B = 0xA0,
            [Instruction("ANA C", 1, 1, 4, "")] ANA_C = 0xA1,
            [Instruction("ANA D", 1, 1, 4, "")] ANA_D = 0xA2,
            [Instruction("ANA E", 1, 1, 4, "")] ANA_E = 0xA3,
            [Instruction("ANA H", 1, 1, 4, "")] ANA_H = 0xA4,
            [Instruction("ANA L", 1, 1, 4, "")] ANA_L = 0xA5,
            [Instruction("ANA M", 1, 2, 7, "")] ANA_M = 0xA6,
            [Instruction("ANA A", 1, 1, 4, "")] ANA_A = 0xA7,
            [Instruction("XRA B", 1, 1, 4, "")] XRA_B = 0xA8,
            [Instruction("XRA C", 1, 1, 4, "")] XRA_C = 0xA9,
            [Instruction("XRA D", 1, 1, 4, "")] XRA_D = 0xAA,
            [Instruction("XRA E", 1, 1, 4, "")] XRA_E = 0xAB,
            [Instruction("XRA H", 1, 1, 4, "")] XRA_H = 0xAC,
            [Instruction("XRA L", 1, 1, 4, "")] XRA_L = 0xAD,
            [Instruction("XRA M", 1, 2, 7, "")] XRA_M = 0xAE,
            [Instruction("XRA A", 1, 1, 4, "")] XRA_A = 0xAF,
            [Instruction("ORA B", 1, 1, 4, "")] ORA_B = 0xB0,
            [Instruction("ORA C", 1, 1, 4, "")] ORA_C = 0xB1,
            [Instruction("ORA D", 1, 1, 4, "")] ORA_D = 0xB2,
            [Instruction("ORA E", 1, 1, 4, "")] ORA_E = 0xB3,
            [Instruction("ORA H", 1, 1, 4, "")] ORA_H = 0xB4,
            [Instruction("ORA L", 1, 1, 4, "")] ORA_L = 0xB5,
            [Instruction("ORA M", 1, 2, 7, "")] ORA_M = 0xB6,
            [Instruction("ORA A", 1, 1, 4, "")] ORA_A = 0xB7,
            [Instruction("CMP B", 1, 1, 4, "")] CMP_B = 0xB8,
            [Instruction("CMP C", 1, 1, 4, "")] CMP_C = 0xB9,
            [Instruction("CMP D", 1, 1, 4, "")] CMP_D = 0xBA,
            [Instruction("CMP E", 1, 1, 4, "")] CMP_E = 0xBB,
            [Instruction("CMP H", 1, 1, 4, "")] CMP_H = 0xBC,
            [Instruction("CMP L", 1, 1, 4, "")] CMP_L = 0xBD,
            [Instruction("CMP M", 1, 2, 7, "")] CMP_M = 0xBE,
            [Instruction("CMP A", 1, 1, 4, "")] CMP_A = 0xBF,
            [Instruction("RNZ", 1, 3, 10, "")] RNZ = 0xC0,
            [Instruction("POP B", 1, 3, 10, "")] POP_B = 0xC1,
            [Instruction("JNZ", 3, 3, 10, "")] JNZ = 0xC2,
            [Instruction("JMP", 3, 3, 10, "")] JMP = 0xC3,
            [Instruction("CNZ", 3, 5, 18, "")] CNZ = 0xC4,
            [Instruction("PUSH B", 1, 3, 12, "")] PUSH_B = 0xC5,
            [Instruction("ADI", 2, 2, 7, "")] ADI = 0xC6,
            [Instruction("RST 0", 1, 3, 12, "")] RST_0 = 0xC7,
            [Instruction("RZ", 1, 3, 10, "")] RZ = 0xC8,
            [Instruction("RET", 1, 3, 10, "")] RET = 0xC9,
            [Instruction("JZ", 3, 3, 10, "")] JZ = 0xCA,
            [Instruction("", 0, 0, 0, "")] UNKN_CB = 0xCB,
            [Instruction("CZ", 3, 5, 18, "")] CZ = 0xCC,
            [Instruction("CALL", 3, 5, 18, "")] CALL = 0xCD,
            [Instruction("ACI", 2, 2, 7, "")] ACI = 0xCE,
            [Instruction("RST 1", 1, 3, 12, "")] RST_1 = 0xCF,
            [Instruction("RNC", 1, 3, 10, "")] RNC = 0xD0,
            [Instruction("POP D", 1, 3, 10, "")] POP_D = 0xD1,
            [Instruction("JNC", 3, 3, 10, "")] JNC = 0xD2,
            [Instruction("OUT", 2, 3, 10, "")] OUT = 0xD3,
            [Instruction("CNC", 3, 5, 18, "")] CNC = 0xD4,
            [Instruction("PUSH", 1, 3, 12, "")] PUSH_D = 0xD5,
            [Instruction("SUI", 2, 2, 7, "")] SUI = 0xD6,
            [Instruction("RST 2", 1, 3, 12, "")] RST_2 = 0xD7,
            [Instruction("RC", 1, 3, 10, "")] RC = 0xD8,
            [Instruction("", 0, 0, 0, "")] UNKN_D9 = 0xD9,
            [Instruction("JC", 3, 3, 10, "")] JC = 0xDA,
            [Instruction("IN", 2, 3, 10, "")] IN = 0xDB,
            [Instruction("CC", 3, 5, 18, "")] CC = 0xDC,
            [Instruction("", 0, 0, 0, "")] UNKN_0D = 0xDD,
            [Instruction("SBI", 2, 2, 7, "")] SBI = 0xDE,
            [Instruction("RST 3", 1, 3, 12, "")] RST_3 = 0xDF,
            [Instruction("RPO", 1, 3, 10, "")] RPO = 0xE0,
            [Instruction("POP H", 1, 3, 10, "")] POP_H = 0xE1,
            [Instruction("JPO", 1, 3, 10, "")] JPO = 0xE2,
            [Instruction("XTHL", 1, 5, 16, "")] XTHL = 0xE3,
            [Instruction("CPO", 3, 5, 18, "")] CPO = 0xE4,
            [Instruction("PUSH H", 1, 3, 12, "")] PUSH_H = 0xE5,
            [Instruction("ANI", 2, 2, 7, "")] ANI = 0xE6,
            [Instruction("RST 4", 1, 3, 12, "")] RST_4 = 0xE7,
            [Instruction("RPE", 1, 3, 10, "")] RPE = 0xE8,
            [Instruction("PCHL", 1, 1, 6, "")] PCHL = 0xE9,
            [Instruction("JPE", 3, 3, 10, "")] JPE = 0xEA,
            [Instruction("XCHG", 1, 1, 4, "")] XCHG = 0xEB,
            [Instruction("CPE", 3, 5, 18, "")] CPE = 0xEC,
            [Instruction("", 0, 0, 0, "")] UNKN_ED = 0xED,
            [Instruction("XRI", 2, 2, 7, "")] XRI = 0xEE,
            [Instruction("RST 5", 1, 3, 12, "")] RST_5 = 0xEF,
            [Instruction("RP", 1, 3, 10, "")] RP = 0xF0,
            [Instruction("POP PSW", 1, 3, 10, "")] POP_PSW = 0xF1,
            [Instruction("JP", 3, 3, 10, "")] JP = 0xF2,
            [Instruction("DI", 1, 1, 4, "")] DI = 0xF3,
            [Instruction("CP", 3, 5, 18, "")] CP = 0xF4,
            [Instruction("PUSH PSW", 1, 3, 12, "")] PUSH_PSW = 0xF5,
            [Instruction("ORI", 2, 2, 7, "")] ORI = 0xF6,
            [Instruction("RST 6", 1, 3, 12, "")] RST_6 = 0xF7,
            [Instruction("RM", 1, 3, 10, "")] RM = 0xF8,
            [Instruction("SPHL", 1, 1, 6, "")] SPHL = 0xF9,
            [Instruction("JM", 3, 3, 10, "")] JM = 0xFA,
            [Instruction("EI", 1, 1, 4, "")] EI = 0xFB,
            [Instruction("CM", 3, 5, 18, "")] CM = 0xFC,
            [Instruction("", 0, 0, 0, "")] UNKN_FD = 0xFD,
            [Instruction("CPI", 2, 2, 7, "")] CPI = 0xFE,
            [Instruction("RST 7", 1, 3, 12, "")] RST_7 = 0xFF
        }
    }
}