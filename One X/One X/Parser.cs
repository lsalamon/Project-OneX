﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace One_X {
    public class Parser {
        private static string labelPattern = "[ \\t]*(?:(?<label>[A-Z_][\\w]+)[ \\t]*:)?[ \\t]*";
        public static string oneByte = "(?<range>";
        public static string twoByte = "(?<range>";
        public static string threeByte = "(?<range>";
        public static string pattern = "^" + labelPattern + buildInstructionPattern() + "[ \\t]*$";
        /**<summary>
         * This is the Enum to store the level of Debug. 
         * </summary>
         */
        public enum DebugLevel {
            Information,
            Error,
            Verbose
        }
        /**<summary>
         * This is the list of Tuple for Error List. 
         * </summary>
         */
        List<(DebugLevel debugLevel, int lineInd, int colInd, int length)> errorList = new List<(DebugLevel debugLevel, int lineInd, int colInd, int length)>();
        Dictionary<ushort, Instruction> instructions = new Dictionary<ushort, Instruction>();

        Dictionary<string, ushort> labels = new Dictionary<string, ushort>();

        List<int> memory = new List<int>();// To generate the warning. 

        List<(int lineInd, int colInd, int length, ushort address, string reference)> tempReference = 
            new List<(int lineInd, int colInd, int length, ushort address, string reference)>();

        /**<summary>
         * Variables to store the names of the Regex Groups. 
         *</summary>
         */
        string LABEL = "label", REFERENCE = "reference", ONEBYTE_INSTRUCTION = "oneByte", TWOBYTE_INSTRUCTION = "twoByte", THREEBYTE_INSTRUCTION = "threeByte",
            LIT_BYTE = "litByte", LIT_USHORT = "litUshort";
        /**<summary>
         * A local Variable to Store Labels.
         * </summary>
         */
        string label = string.Empty;
        /**<summary>
         * A Local Variable to store the instrctions. 
         * </summary>
         */
        string instruction_name = string.Empty;

        string instruction_type = string.Empty;
        /**<summary>
         * A Local variable to store the right literal.
         * </summary>
         */
        string rightLit = string.Empty;
        ushort right_LIT;

        string rightLit_type = string.Empty;

        char[] newLine = { '\n' };

        ushort address;     //Starting Address. 

        public Parser(ushort address) {
            this.address = address; //Setting address.
            instructions.Clear();
            labels.Clear();
        }

        private static Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static string buildInstructionPattern() {
            string[] rxps = new string[3];

            foreach (var inst in Instruction.list) {
                rxps[inst.Bytes - 1] += inst.Name;
                if (inst.Bytes > 1) {
                    if (inst.Name.Contains(" ")) {
                        rxps[inst.Bytes - 1] += ",";
                    }
                    else {
                        rxps[inst.Bytes - 1] += "[ \\t]";
                    }
                }
                rxps[inst.Bytes - 1] += "|";
            }

            oneByte += rxps[0].Remove(rxps[0].Length - 1) + ")";
            twoByte += rxps[1].Remove(rxps[1].Length - 1) + ")[ \\t]*(?<litByte>[0-9A-F]{1,2}H?)";
            threeByte += rxps[2].Remove(rxps[2].Length - 1) + ")[ \\t]*(?:(?<litShort>[0-9A-F]{1,4}H?)|(?<reference>[\\w]+))";

            rxps[0] = oneByte.Replace("<range>", "<oneByte>");
            rxps[1] = twoByte.Replace("<range>", "<twoByte>");
            rxps[2] = threeByte.Replace("<range>", "<threeByte>");

            return "(?<instruction>" + string.Join("|", rxps) + ")";
        }

        public void parse(string code) {
            string[] line = code.Split(newLine);
            int i, lineNum = line.Length;
            for (i = 0; i < lineNum; i++) {
                Match match = RegexHelper.rxInstructionLine.Match(line[i]);
                label = match.Groups[LABEL].Value;
                if (!label.Equals(string.Empty)) {
                    labels.Add(label, 00);      // Adding the labels only. 
                }
            }
            //Parsing the Whole Code.         
            label = string.Empty;
            for (i = 0; i < lineNum; i++) {
                Match match = RegexHelper.rxInstructionLine.Match(line[i]);
                if (!match.Groups[LABEL].Value.Equals(string.Empty)) {
                    label = match.Groups[LABEL].Value;
                }
                if (label != string.Empty) {
                    if (!labels.ContainsKey(label)) {
                        labels.Add(label, address);
                    }
                    else if (labels.ContainsKey(label)) {
                        labels[label] = address;
                    }
                }
                //Getting the instruction.                   
                if (match.Groups[ONEBYTE_INSTRUCTION].Value != string.Empty) {
                    instruction_name = match.Groups[ONEBYTE_INSTRUCTION].Value.Trim();
                    instruction_type = ONEBYTE_INSTRUCTION;
                }
                else if (match.Groups[TWOBYTE_INSTRUCTION].Value != string.Empty) {
                    instruction_name = match.Groups[TWOBYTE_INSTRUCTION].Value.Trim();
                    instruction_type = TWOBYTE_INSTRUCTION;
                }
                else if (match.Groups[THREEBYTE_INSTRUCTION].Value != string.Empty) {
                    instruction_name = match.Groups[THREEBYTE_INSTRUCTION].Value.Trim();
                    instruction_type = THREEBYTE_INSTRUCTION;
                }

                if (instruction_name == string.Empty) {
                    if (line[i].Length != label.Length) {
                        errorList.Add((DebugLevel.Error, i, label.Length + 1, (line[i].Length - label.Length))); //Lable with Invalid Characters. 
                    }
                    continue; //Blank Label. 
                }

                //Getting the Right Literal. 
                if (match.Groups[LIT_BYTE].Value != string.Empty) {
                    rightLit = match.Groups[LIT_BYTE].Value;
                    rightLit_type = LIT_BYTE;
                }
                else if (match.Groups[LIT_USHORT].Value != string.Empty) {
                    rightLit = match.Groups[LIT_USHORT].Value;
                    rightLit_type = LIT_USHORT;
                    if (!memory.Contains(Convert.ToInt32(rightLit))) {
                        memory.Add(Convert.ToInt32(rightLit));
                    }
                    else {
                        errorList.Add((DebugLevel.Information, i, (label.Length + instruction_name.Length), rightLit.Length));
                    }
                }
                else if (match.Groups[REFERENCE].Value != string.Empty) {
                    rightLit = match.Groups[REFERENCE].Value;
                    rightLit_type = REFERENCE;
                }
                //Checking for MisMatch ERROR.
                /*if (instruction_type == ONEBYTE_INSTRUCTION && (rightLit_type == LIT_USHORT || rightLit_type == REFERENCE)) {
                    errorList.Add((DebugLevel.Error, i, (label.Length + instruction_name.Length), rightLit.Length));
                }*/
                else if (instruction_type == TWOBYTE_INSTRUCTION && (rightLit_type == REFERENCE || rightLit_type == LIT_USHORT)) {
                    errorList.Add((DebugLevel.Error, i, (label.Length + instruction_name.Length), rightLit.Length));
                }
                if (rightLit_type != REFERENCE) {
                    ushort right_LIT = ushort.Parse(rightLit);
                }
                else if (rightLit_type == REFERENCE) {
                    tempReference.Add((i, label.Length + instruction_name.Length, rightLit.Length, address, rightLit)); //Adding the temporary reference. 
                }
                if (rightLit_type != REFERENCE) {
                    Instruction inst = null;
                    foreach(Instruction ins in Instruction.list) {
                        if (ins.Name == instruction_name) {
                            inst = ins;                           
                            break;
                        }
                    }
                    //Adding the instructions. 
                    instructions
                        .Add(address,new Instruction(inst.Name, inst.Bytes, inst.MCycles, inst.TStates, inst.method, BitHelper.ToBytes(ushort.Parse(rightLit))));
                    inst = null;
                }
                else {
                    instructions.Add(address, new Instruction(instruction_name, 0, 0, 0,""));//Dummy data for instructions with reference. 
                }
            }
            addReferences();
        }
        private void addReferences() {
            foreach(var refer in tempReference) {
                foreach(var label in labels) {
                    if (refer.reference == label.Key) {
                        ushort refAddress = label.Value;
                        Instruction originalInstruction = null;
                        foreach(Instruction i in Instruction.list) {
                            if (instructions[label.Value].Name == i.Name) {
                                originalInstruction = i;
                                break;
                            }
                        }
                        instructions[label.Value].Bytes = originalInstruction.Bytes;
                        instructions[label.Value].MCycles = originalInstruction.MCycles;
                        instructions[label.Value].TStates = originalInstruction.TStates;
                        instructions[label.Value].method = originalInstruction.method;
                        instructions[label.Value].Arguments = BitHelper.ToBytes(refAddress);
                    }
                }
            }
        }
    }
}