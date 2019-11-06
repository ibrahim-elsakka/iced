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

#if !NO_DECODER
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal {
	sealed class OpCodeHandler_D3NOW : OpCodeHandlerModRM {
		internal static readonly Code[] CodeValues = CreateCodeValues();

		static Code[] CreateCodeValues() {
			var result = new Code[0x100];
			Static.Assert(Code.INVALID == 0 ? 0 : -1);
			result[0x0C] = Code.D3NOW_Pi2fw_mm_mmm64;
			result[0x0D] = Code.D3NOW_Pi2fd_mm_mmm64;
			result[0x1C] = Code.D3NOW_Pf2iw_mm_mmm64;
			result[0x1D] = Code.D3NOW_Pf2id_mm_mmm64;
			result[0x86] = Code.D3NOW_Pfrcpv_mm_mmm64;
			result[0x87] = Code.D3NOW_Pfrsqrtv_mm_mmm64;
			result[0x8A] = Code.D3NOW_Pfnacc_mm_mmm64;
			result[0x8E] = Code.D3NOW_Pfpnacc_mm_mmm64;
			result[0x90] = Code.D3NOW_Pfcmpge_mm_mmm64;
			result[0x94] = Code.D3NOW_Pfmin_mm_mmm64;
			result[0x96] = Code.D3NOW_Pfrcp_mm_mmm64;
			result[0x97] = Code.D3NOW_Pfrsqrt_mm_mmm64;
			result[0x9A] = Code.D3NOW_Pfsub_mm_mmm64;
			result[0x9E] = Code.D3NOW_Pfadd_mm_mmm64;
			result[0xA0] = Code.D3NOW_Pfcmpgt_mm_mmm64;
			result[0xA4] = Code.D3NOW_Pfmax_mm_mmm64;
			result[0xA6] = Code.D3NOW_Pfrcpit1_mm_mmm64;
			result[0xA7] = Code.D3NOW_Pfrsqit1_mm_mmm64;
			result[0xAA] = Code.D3NOW_Pfsubr_mm_mmm64;
			result[0xAE] = Code.D3NOW_Pfacc_mm_mmm64;
			result[0xB0] = Code.D3NOW_Pfcmpeq_mm_mmm64;
			result[0xB4] = Code.D3NOW_Pfmul_mm_mmm64;
			result[0xB6] = Code.D3NOW_Pfrcpit2_mm_mmm64;
			result[0xB7] = Code.D3NOW_Pmulhrw_mm_mmm64;
			result[0xBB] = Code.D3NOW_Pswapd_mm_mmm64;
			result[0xBF] = Code.D3NOW_Pavgusb_mm_mmm64;
			return result;
		}

		readonly Code[] codeValues = CodeValues;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.MM0;
			uint ib;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)state.rm + Register.MM0;
				ib = decoder.ReadByte();
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
				ib = decoder.ReadByte();
			}
			var code = codeValues[(int)ib];
			instruction.InternalCode = code;
			if (code == Code.INVALID)
				decoder.SetInvalidInstruction();
		}
	}
}
#endif
