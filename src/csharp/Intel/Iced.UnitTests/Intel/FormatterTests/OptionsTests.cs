/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#if !NO_GAS || !NO_INTEL || !NO_MASM || !NO_NASM
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	// GENERATOR-BEGIN: OptionsProps
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	enum OptionsProps {
		AddLeadingZeroToHexNumbers,
		AlwaysShowScale,
		AlwaysShowSegmentRegister,
		BinaryDigitGroupSize,
		BinaryPrefix,
		BinarySuffix,
		BranchLeadingZeroes,
		DecimalDigitGroupSize,
		DecimalPrefix,
		DecimalSuffix,
		DigitSeparator,
		DisplacementLeadingZeroes,
		FirstOperandCharIndex,
		GasNakedRegisters,
		GasShowMnemonicSizeSuffix,
		GasSpaceAfterMemoryOperandComma,
		HexDigitGroupSize,
		HexPrefix,
		HexSuffix,
		IP,
		LeadingZeroes,
		MasmAddDsPrefix32,
		MemorySizeOptions,
		NasmShowSignExtendedImmediateSize,
		NumberBase,
		OctalDigitGroupSize,
		OctalPrefix,
		OctalSuffix,
		PreferST0,
		RipRelativeAddresses,
		ScaleBeforeIndex,
		ShowBranchSize,
		ShowZeroDisplacements,
		SignedImmediateOperands,
		SignedMemoryDisplacements,
		SmallHexNumbersInDecimal,
		SpaceAfterMemoryBracket,
		SpaceAfterOperandSeparator,
		SpaceBetweenMemoryAddOperators,
		SpaceBetweenMemoryMulOperators,
		TabSize,
		UpperCaseAll,
		UpperCaseDecorators,
		UpperCaseHex,
		UpperCaseKeywords,
		UpperCaseMnemonics,
		UpperCasePrefixes,
		UpperCaseRegisters,
		UsePseudoOps,
	}
	// GENERATOR-END: OptionsProps

	public readonly struct OptionsInstructionInfo {
		public readonly int Bitness;
		public readonly string HexBytes;
		public readonly Code Code;
		readonly List<(OptionsProps property, object value)> properties;
		internal OptionsInstructionInfo(int bitness, string hexBytes, Code code, List<(OptionsProps property, object value)> properties) {
			Bitness = bitness;
			HexBytes = hexBytes;
			Code = code;
			this.properties = properties;
		}

		internal void Initialize(FormatterOptions options) {
			foreach (var info in properties) {
				switch (info.property) {
				case OptionsProps.AddLeadingZeroToHexNumbers: options.AddLeadingZeroToHexNumbers = (bool)info.value; break;
				case OptionsProps.AlwaysShowScale: options.AlwaysShowScale = (bool)info.value; break;
				case OptionsProps.AlwaysShowSegmentRegister: options.AlwaysShowSegmentRegister = (bool)info.value; break;
				case OptionsProps.BinaryDigitGroupSize: options.BinaryDigitGroupSize = (int)info.value; break;
				case OptionsProps.BinaryPrefix: options.BinaryPrefix = (string)info.value; break;
				case OptionsProps.BinarySuffix: options.BinarySuffix = (string)info.value; break;
				case OptionsProps.BranchLeadingZeroes: options.BranchLeadingZeroes = (bool)info.value; break;
				case OptionsProps.DecimalDigitGroupSize: options.DecimalDigitGroupSize = (int)info.value; break;
				case OptionsProps.DecimalPrefix: options.DecimalPrefix = (string)info.value; break;
				case OptionsProps.DecimalSuffix: options.DecimalSuffix = (string)info.value; break;
				case OptionsProps.DigitSeparator: options.DigitSeparator = (string)info.value; break;
				case OptionsProps.DisplacementLeadingZeroes: options.DisplacementLeadingZeroes = (bool)info.value; break;
				case OptionsProps.FirstOperandCharIndex: options.FirstOperandCharIndex = (int)info.value; break;
#if !NO_GAS
				case OptionsProps.GasNakedRegisters: ((GasFormatterOptions)options).NakedRegisters = (bool)info.value; break;
				case OptionsProps.GasShowMnemonicSizeSuffix: ((GasFormatterOptions)options).ShowMnemonicSizeSuffix = (bool)info.value; break;
				case OptionsProps.GasSpaceAfterMemoryOperandComma: ((GasFormatterOptions)options).SpaceAfterMemoryOperandComma = (bool)info.value; break;
#endif
				case OptionsProps.HexDigitGroupSize: options.HexDigitGroupSize = (int)info.value; break;
				case OptionsProps.HexPrefix: options.HexPrefix = (string)info.value; break;
				case OptionsProps.HexSuffix: options.HexSuffix = (string)info.value; break;
				case OptionsProps.LeadingZeroes: options.LeadingZeroes = (bool)info.value; break;
#if !NO_MASM
				case OptionsProps.MasmAddDsPrefix32: ((MasmFormatterOptions)options).AddDsPrefix32 = (bool)info.value; break;
#endif
				case OptionsProps.MemorySizeOptions: options.MemorySizeOptions = (MemorySizeOptions)info.value; break;
#if !NO_NASM
				case OptionsProps.NasmShowSignExtendedImmediateSize: ((NasmFormatterOptions)options).ShowSignExtendedImmediateSize = (bool)info.value; break;
#endif
				case OptionsProps.NumberBase: options.NumberBase = (NumberBase)info.value; break;
				case OptionsProps.OctalDigitGroupSize: options.OctalDigitGroupSize = (int)info.value; break;
				case OptionsProps.OctalPrefix: options.OctalPrefix = (string)info.value; break;
				case OptionsProps.OctalSuffix: options.OctalSuffix = (string)info.value; break;
				case OptionsProps.PreferST0: options.PreferST0 = (bool)info.value; break;
				case OptionsProps.RipRelativeAddresses: options.RipRelativeAddresses = (bool)info.value; break;
				case OptionsProps.ScaleBeforeIndex: options.ScaleBeforeIndex = (bool)info.value; break;
				case OptionsProps.ShowBranchSize: options.ShowBranchSize = (bool)info.value; break;
				case OptionsProps.ShowZeroDisplacements: options.ShowZeroDisplacements = (bool)info.value; break;
				case OptionsProps.SignedImmediateOperands: options.SignedImmediateOperands = (bool)info.value; break;
				case OptionsProps.SignedMemoryDisplacements: options.SignedMemoryDisplacements = (bool)info.value; break;
				case OptionsProps.SmallHexNumbersInDecimal: options.SmallHexNumbersInDecimal = (bool)info.value; break;
				case OptionsProps.SpaceAfterMemoryBracket: options.SpaceAfterMemoryBracket = (bool)info.value; break;
				case OptionsProps.SpaceAfterOperandSeparator: options.SpaceAfterOperandSeparator = (bool)info.value; break;
				case OptionsProps.SpaceBetweenMemoryAddOperators: options.SpaceBetweenMemoryAddOperators = (bool)info.value; break;
				case OptionsProps.SpaceBetweenMemoryMulOperators: options.SpaceBetweenMemoryMulOperators = (bool)info.value; break;
				case OptionsProps.TabSize: options.TabSize = (int)info.value; break;
				case OptionsProps.UpperCaseAll: options.UpperCaseAll = (bool)info.value; break;
				case OptionsProps.UpperCaseDecorators: options.UpperCaseDecorators = (bool)info.value; break;
				case OptionsProps.UpperCaseHex: options.UpperCaseHex = (bool)info.value; break;
				case OptionsProps.UpperCaseKeywords: options.UpperCaseKeywords = (bool)info.value; break;
				case OptionsProps.UpperCaseMnemonics: options.UpperCaseMnemonics = (bool)info.value; break;
				case OptionsProps.UpperCasePrefixes: options.UpperCasePrefixes = (bool)info.value; break;
				case OptionsProps.UpperCaseRegisters: options.UpperCaseRegisters = (bool)info.value; break;
				case OptionsProps.UsePseudoOps: options.UsePseudoOps = (bool)info.value; break;
				}
			}
		}

		internal void Initialize(Decoder decoder) {
			foreach (var info in properties) {
				switch (info.property) {
				case OptionsProps.IP:
					decoder.IP = (ulong)info.value;
					break;
				}
			}
		}
	}

	public abstract class OptionsTests {
		protected static IEnumerable<object[]> GetFormatData(string formatterDir, string formattedStringsFile, string optionsFile = null) {
			OptionsInstructionInfo[] infos;
			if (optionsFile is null)
				infos = FormatterOptionsTests.AllInfos;
			else {
				var infosFilename = FileUtils.GetFormatterFilename(Path.Combine(formatterDir, optionsFile));
				infos = OptionsTestsReader.ReadFile(infosFilename).ToArray();
			}
			var formattedStrings = FileUtils.ReadRawStrings(Path.Combine(formatterDir, formattedStringsFile)).ToArray();
			if (infos.Length != formattedStrings.Length)
				throw new ArgumentException($"(infos.Length) {infos.Length} != (formattedStrings.Length) {formattedStrings.Length} . infos[0].HexBytes = {(infos.Length == 0 ? "<EMPTY>" : infos[0].HexBytes)} & formattedStrings[0] = {(formattedStrings.Length == 0 ? "<EMPTY>" : formattedStrings[0])}");
			var res = new object[infos.Length][];
			for (int i = 0; i < infos.Length; i++)
				res[i] = new object[3] { i, infos[i], formattedStrings[i] };
			return res;
		}

		protected void FormatBase(int index, OptionsInstructionInfo info, string formattedString, Formatter formatter) {
			info.Initialize(formatter.Options);
			FormatterTestUtils.SimpleFormatTest(info.Bitness, info.HexBytes, info.Code, DecoderOptions.None, formattedString, formatter, decoder => info.Initialize(decoder));
		}

		static IEnumerable<T> GetEnumValues<T>() where T : struct {
			var t = typeof(T);
			if (!t.IsEnum)
				throw new InvalidOperationException();
			foreach (var value in Enum.GetValues(t))
				yield return (T)value;
		}

		protected void TestOptionsBase(FormatterOptions options) {
			{
				int min = int.MaxValue, max = int.MinValue;
				foreach (var value in GetEnumValues<NumberBase>()) {
					min = Math.Min(min, (int)value);
					max = Math.Max(max, (int)value);
					options.NumberBase = value;
				}
				Assert.Throws<ArgumentOutOfRangeException>(() => options.NumberBase = (NumberBase)(min - 1));
				Assert.Throws<ArgumentOutOfRangeException>(() => options.NumberBase = (NumberBase)(max + 1));
				Assert.Throws<ArgumentOutOfRangeException>(() => options.NumberBase = (NumberBase)int.MinValue);
				Assert.Throws<ArgumentOutOfRangeException>(() => options.NumberBase = (NumberBase)int.MaxValue);
			}

			{
				int min = int.MaxValue, max = int.MinValue;
				foreach (var value in GetEnumValues<MemorySizeOptions>()) {
					min = Math.Min(min, (int)value);
					max = Math.Max(max, (int)value);
					options.MemorySizeOptions = value;
				}
				Assert.Throws<ArgumentOutOfRangeException>(() => options.MemorySizeOptions = (MemorySizeOptions)(min - 1));
				Assert.Throws<ArgumentOutOfRangeException>(() => options.MemorySizeOptions = (MemorySizeOptions)(max + 1));
				Assert.Throws<ArgumentOutOfRangeException>(() => options.MemorySizeOptions = (MemorySizeOptions)int.MinValue);
				Assert.Throws<ArgumentOutOfRangeException>(() => options.MemorySizeOptions = (MemorySizeOptions)int.MaxValue);
			}
		}
	}
}
#endif