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

#if !NO_ENCODER
using System;
using System.Diagnostics;
using System.Text;

namespace Iced.Intel.EncoderInternal {
	struct InstructionFormatter {
		readonly OpCodeInfo opCode;
		readonly StringBuilder sb;
		readonly int r32_count;
		readonly int r64_count;
		readonly int bnd_count;
		readonly int startOpIndex;
		int r32_index, r64_index, bnd_index;
		int k_index;
		int vec_index;
		int opCount;
		// true: k2 {k1}, false: k1 {k2}
		readonly bool opMaskIsK1;
		readonly bool noVecIndex;
		readonly bool swapVecIndex12;

		int GetKIndex() {
			k_index++;
			if (opMaskIsK1) {
				if (k_index == 1)
					return 2;
				if (k_index == 2)
					return 1;
			}
			return k_index;
		}

		int GetBndIndex() {
			if (bnd_count <= 1)
				return 0;
			bnd_index++;
			return bnd_index;
		}

		int GetVecIndex() {
			if (noVecIndex)
				return 0;
			vec_index++;
			if (swapVecIndex12) {
				if (vec_index == 1)
					return 2;
				if (vec_index == 2)
					return 1;
			}
			return vec_index;
		}

		public InstructionFormatter(OpCodeInfo opCode, StringBuilder sb) {
			this.opCode = opCode;
			this.sb = sb;
			noVecIndex = false;
			swapVecIndex12 = false;
			startOpIndex = 0;
			bnd_count = 0;
			r32_count = 0;
			r64_count = 0;
			r32_index = 0;
			r64_index = 0;
			k_index = 0;
			vec_index = 0;
			bnd_index = 0;
			opCount = opCode.OpCount;
			opMaskIsK1 = false;
			if ((opCode.Op0Kind == OpCodeOperandKind.k_reg || opCode.Op0Kind == OpCodeOperandKind.kp1_reg) && opCode.OpCount > 2)
				vec_index++;
			switch (opCode.Code) {
			case Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8:
			case Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8:
			case Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8:
			case Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8:
			case Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8:
			case Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8:
			case Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8:
			case Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8:
			case Code.EVEX_Vptestmb_k_k1_xmm_xmmm128:
			case Code.EVEX_Vptestmb_k_k1_ymm_ymmm256:
			case Code.EVEX_Vptestmb_k_k1_zmm_zmmm512:
			case Code.EVEX_Vptestmw_k_k1_xmm_xmmm128:
			case Code.EVEX_Vptestmw_k_k1_ymm_ymmm256:
			case Code.EVEX_Vptestmw_k_k1_zmm_zmmm512:
			case Code.EVEX_Vptestnmb_k_k1_xmm_xmmm128:
			case Code.EVEX_Vptestnmb_k_k1_ymm_ymmm256:
			case Code.EVEX_Vptestnmb_k_k1_zmm_zmmm512:
			case Code.EVEX_Vptestnmw_k_k1_xmm_xmmm128:
			case Code.EVEX_Vptestnmw_k_k1_ymm_ymmm256:
			case Code.EVEX_Vptestnmw_k_k1_zmm_zmmm512:
			case Code.EVEX_Vptestmd_k_k1_xmm_xmmm128b32:
			case Code.EVEX_Vptestmd_k_k1_ymm_ymmm256b32:
			case Code.EVEX_Vptestmd_k_k1_zmm_zmmm512b32:
			case Code.EVEX_Vptestmq_k_k1_xmm_xmmm128b64:
			case Code.EVEX_Vptestmq_k_k1_ymm_ymmm256b64:
			case Code.EVEX_Vptestmq_k_k1_zmm_zmmm512b64:
			case Code.EVEX_Vptestnmd_k_k1_xmm_xmmm128b32:
			case Code.EVEX_Vptestnmd_k_k1_ymm_ymmm256b32:
			case Code.EVEX_Vptestnmd_k_k1_zmm_zmmm512b32:
			case Code.EVEX_Vptestnmq_k_k1_xmm_xmmm128b64:
			case Code.EVEX_Vptestnmq_k_k1_ymm_ymmm256b64:
			case Code.EVEX_Vptestnmq_k_k1_zmm_zmmm512b64:
				opMaskIsK1 = true;
				break;
			case Code.VEX_Vpextrw_r32m16_xmm_imm8:
			case Code.VEX_Vpextrw_r64m16_xmm_imm8:
			case Code.EVEX_Vpextrw_r32m16_xmm_imm8:
			case Code.EVEX_Vpextrw_r64m16_xmm_imm8:
			case Code.VEX_Vmovmskpd_r32_xmm:
			case Code.VEX_Vmovmskpd_r64_xmm:
			case Code.VEX_Vmovmskpd_r32_ymm:
			case Code.VEX_Vmovmskpd_r64_ymm:
			case Code.VEX_Vmovmskps_r32_xmm:
			case Code.VEX_Vmovmskps_r64_xmm:
			case Code.VEX_Vmovmskps_r32_ymm:
			case Code.VEX_Vmovmskps_r64_ymm:
			case Code.Pextrb_r32m8_xmm_imm8:
			case Code.Pextrb_r64m8_xmm_imm8:
			case Code.Pextrd_rm32_xmm_imm8:
			case Code.Pextrq_rm64_xmm_imm8:
			case Code.VEX_Vpextrb_r32m8_xmm_imm8:
			case Code.VEX_Vpextrb_r64m8_xmm_imm8:
			case Code.VEX_Vpextrd_rm32_xmm_imm8:
			case Code.VEX_Vpextrq_rm64_xmm_imm8:
			case Code.EVEX_Vpextrb_r32m8_xmm_imm8:
			case Code.EVEX_Vpextrb_r64m8_xmm_imm8:
			case Code.EVEX_Vpextrd_rm32_xmm_imm8:
			case Code.EVEX_Vpextrq_rm64_xmm_imm8:
				vec_index++;
				break;
			case Code.Pxor_mm_mmm64:
			case Code.Punpckldq_mm_mmm32:
			case Code.Punpcklwd_mm_mmm32:
			case Code.Punpcklbw_mm_mmm32:
			case Code.Punpckhdq_mm_mmm64:
			case Code.Punpckhwd_mm_mmm64:
			case Code.Punpckhbw_mm_mmm64:
			case Code.Psubusb_mm_mmm64:
			case Code.Psubusw_mm_mmm64:
			case Code.Psubsw_mm_mmm64:
			case Code.Psubsb_mm_mmm64:
			case Code.Psubd_mm_mmm64:
			case Code.Psubw_mm_mmm64:
			case Code.Psubb_mm_mmm64:
			case Code.Psrlq_mm_imm8:
			case Code.Psrlq_mm_mmm64:
			case Code.Psrld_mm_imm8:
			case Code.Psrld_mm_mmm64:
			case Code.Psrlw_mm_imm8:
			case Code.Psrlw_mm_mmm64:
			case Code.Psrad_mm_imm8:
			case Code.Psrad_mm_mmm64:
			case Code.Psraw_mm_imm8:
			case Code.Psraw_mm_mmm64:
			case Code.Psllq_mm_imm8:
			case Code.Psllq_mm_mmm64:
			case Code.Pslld_mm_imm8:
			case Code.Pslld_mm_mmm64:
			case Code.Psllw_mm_mmm64:
			case Code.Por_mm_mmm64:
			case Code.Pmullw_mm_mmm64:
			case Code.Pmulhw_mm_mmm64:
			case Code.Pmovmskb_r32_mm:
			case Code.Pmovmskb_r64_mm:
			case Code.Pmovmskb_r32_xmm:
			case Code.Pmovmskb_r64_xmm:
			case Code.Pmaddwd_mm_mmm64:
			case Code.Pinsrw_mm_r32m16_imm8:
			case Code.Pinsrw_mm_r64m16_imm8:
			case Code.Pinsrw_xmm_r32m16_imm8:
			case Code.Pinsrw_xmm_r64m16_imm8:
			case Code.Pextrw_r32_xmm_imm8:
			case Code.Pextrw_r64_xmm_imm8:
			case Code.Pextrw_r32m16_xmm_imm8:
			case Code.Pextrw_r64m16_xmm_imm8:
			case Code.Pextrw_r32_mm_imm8:
			case Code.Pextrw_r64_mm_imm8:
			case Code.Cvtpd2pi_mm_xmmm128:
			case Code.Cvtpi2pd_xmm_mmm64:
			case Code.Cvtpi2ps_xmm_mmm64:
			case Code.Cvtps2pi_mm_xmmm64:
			case Code.Cvttpd2pi_mm_xmmm128:
			case Code.Cvttps2pi_mm_xmmm64:
			case Code.Movd_mm_rm32:
			case Code.Movq_mm_rm64:
			case Code.Movd_rm32_mm:
			case Code.Movq_rm64_mm:
			case Code.Movd_xmm_rm32:
			case Code.Movq_xmm_rm64:
			case Code.Movd_rm32_xmm:
			case Code.Movq_rm64_xmm:
			case Code.Movdq2q_mm_xmm:
			case Code.Movmskpd_r32_xmm:
			case Code.Movmskpd_r64_xmm:
			case Code.Movmskps_r32_xmm:
			case Code.Movmskps_r64_xmm:
			case Code.Movntq_m64_mm:
			case Code.Movq_mm_mmm64:
			case Code.Movq_mmm64_mm:
			case Code.Movq2dq_xmm_mm:
			case Code.Packuswb_mm_mmm64:
			case Code.Paddb_mm_mmm64:
			case Code.Paddw_mm_mmm64:
			case Code.Paddd_mm_mmm64:
			case Code.Paddq_mm_mmm64:
			case Code.Paddsb_mm_mmm64:
			case Code.Paddsw_mm_mmm64:
			case Code.Paddusb_mm_mmm64:
			case Code.Paddusw_mm_mmm64:
			case Code.Pand_mm_mmm64:
			case Code.Pandn_mm_mmm64:
			case Code.Pcmpeqb_mm_mmm64:
			case Code.Pcmpeqw_mm_mmm64:
			case Code.Pcmpeqd_mm_mmm64:
			case Code.Pcmpgtb_mm_mmm64:
			case Code.Pcmpgtw_mm_mmm64:
			case Code.Pcmpgtd_mm_mmm64:
				noVecIndex = true;
				break;
			case Code.Movapd_xmmm128_xmm:
			case Code.VEX_Vmovapd_xmmm128_xmm:
			case Code.VEX_Vmovapd_ymmm256_ymm:
			case Code.EVEX_Vmovapd_xmmm128_k1z_xmm:
			case Code.EVEX_Vmovapd_ymmm256_k1z_ymm:
			case Code.EVEX_Vmovapd_zmmm512_k1z_zmm:
			case Code.Movaps_xmmm128_xmm:
			case Code.VEX_Vmovaps_xmmm128_xmm:
			case Code.VEX_Vmovaps_ymmm256_ymm:
			case Code.EVEX_Vmovaps_xmmm128_k1z_xmm:
			case Code.EVEX_Vmovaps_ymmm256_k1z_ymm:
			case Code.EVEX_Vmovaps_zmmm512_k1z_zmm:
			case Code.Movdqa_xmmm128_xmm:
			case Code.VEX_Vmovdqa_xmmm128_xmm:
			case Code.VEX_Vmovdqa_ymmm256_ymm:
			case Code.EVEX_Vmovdqa32_xmmm128_k1z_xmm:
			case Code.EVEX_Vmovdqa32_ymmm256_k1z_ymm:
			case Code.EVEX_Vmovdqa32_zmmm512_k1z_zmm:
			case Code.EVEX_Vmovdqa64_xmmm128_k1z_xmm:
			case Code.EVEX_Vmovdqa64_ymmm256_k1z_ymm:
			case Code.EVEX_Vmovdqa64_zmmm512_k1z_zmm:
			case Code.Movdqu_xmmm128_xmm:
			case Code.VEX_Vmovdqu_xmmm128_xmm:
			case Code.VEX_Vmovdqu_ymmm256_ymm:
			case Code.EVEX_Vmovdqu8_xmmm128_k1z_xmm:
			case Code.EVEX_Vmovdqu8_ymmm256_k1z_ymm:
			case Code.EVEX_Vmovdqu8_zmmm512_k1z_zmm:
			case Code.EVEX_Vmovdqu16_xmmm128_k1z_xmm:
			case Code.EVEX_Vmovdqu16_ymmm256_k1z_ymm:
			case Code.EVEX_Vmovdqu16_zmmm512_k1z_zmm:
			case Code.EVEX_Vmovdqu32_xmmm128_k1z_xmm:
			case Code.EVEX_Vmovdqu32_ymmm256_k1z_ymm:
			case Code.EVEX_Vmovdqu32_zmmm512_k1z_zmm:
			case Code.EVEX_Vmovdqu64_xmmm128_k1z_xmm:
			case Code.EVEX_Vmovdqu64_ymmm256_k1z_ymm:
			case Code.EVEX_Vmovdqu64_zmmm512_k1z_zmm:
			case Code.VEX_Vmovhpd_xmm_xmm_m64:
			case Code.EVEX_Vmovhpd_xmm_xmm_m64:
			case Code.VEX_Vmovhps_xmm_xmm_m64:
			case Code.EVEX_Vmovhps_xmm_xmm_m64:
			case Code.VEX_Vmovlpd_xmm_xmm_m64:
			case Code.EVEX_Vmovlpd_xmm_xmm_m64:
			case Code.VEX_Vmovlps_xmm_xmm_m64:
			case Code.EVEX_Vmovlps_xmm_xmm_m64:
			case Code.Movq_xmmm64_xmm:
			case Code.Movss_xmmm32_xmm:
			case Code.Movupd_xmmm128_xmm:
			case Code.VEX_Vmovupd_xmmm128_xmm:
			case Code.VEX_Vmovupd_ymmm256_ymm:
			case Code.EVEX_Vmovupd_xmmm128_k1z_xmm:
			case Code.EVEX_Vmovupd_ymmm256_k1z_ymm:
			case Code.EVEX_Vmovupd_zmmm512_k1z_zmm:
			case Code.Movups_xmmm128_xmm:
			case Code.VEX_Vmovups_xmmm128_xmm:
			case Code.VEX_Vmovups_ymmm256_ymm:
			case Code.EVEX_Vmovups_xmmm128_k1z_xmm:
			case Code.EVEX_Vmovups_ymmm256_k1z_ymm:
			case Code.EVEX_Vmovups_zmmm512_k1z_zmm:
				swapVecIndex12 = true;
				break;
			}
			for (int i = 0; i < opCode.OpCount; i++) {
				switch (opCode.GetOpKind(i)) {
				case OpCodeOperandKind.r32_reg:
				case OpCodeOperandKind.r32_rm:
				case OpCodeOperandKind.r32_opcode:
				case OpCodeOperandKind.r32_vvvv:
					r32_count++;
					break;

				case OpCodeOperandKind.r64_reg:
				case OpCodeOperandKind.r64_rm:
				case OpCodeOperandKind.r64_opcode:
				case OpCodeOperandKind.r64_vvvv:
					r64_count++;
					break;

				case OpCodeOperandKind.bnd_or_mem_mpx:
				case OpCodeOperandKind.bnd_reg:
					bnd_count++;
					break;

				case OpCodeOperandKind.st0:
					if (i == 0) {
						switch (opCode.Code) {
						case Code.Fcom_st0_sti:
						case Code.Fcomp_st0_sti:
						case Code.Fld_st0_sti:
						case Code.Fucom_st0_sti:
						case Code.Fucomp_st0_sti:
						case Code.Fxch_st0_sti:
							startOpIndex = 1;
							break;
						}
					}
					break;

				case OpCodeOperandKind.None:
				case OpCodeOperandKind.farbr2_2:
				case OpCodeOperandKind.farbr4_2:
				case OpCodeOperandKind.mem_offs:
				case OpCodeOperandKind.mem:
				case OpCodeOperandKind.mem_mpx:
				case OpCodeOperandKind.mem_vsib32x:
				case OpCodeOperandKind.mem_vsib64x:
				case OpCodeOperandKind.mem_vsib32y:
				case OpCodeOperandKind.mem_vsib64y:
				case OpCodeOperandKind.mem_vsib32z:
				case OpCodeOperandKind.mem_vsib64z:
				case OpCodeOperandKind.r8_or_mem:
				case OpCodeOperandKind.r16_or_mem:
				case OpCodeOperandKind.r32_or_mem:
				case OpCodeOperandKind.r32_or_mem_mpx:
				case OpCodeOperandKind.r64_or_mem:
				case OpCodeOperandKind.r64_or_mem_mpx:
				case OpCodeOperandKind.mm_or_mem:
				case OpCodeOperandKind.xmm_or_mem:
				case OpCodeOperandKind.ymm_or_mem:
				case OpCodeOperandKind.zmm_or_mem:
				case OpCodeOperandKind.k_or_mem:
				case OpCodeOperandKind.r8_reg:
				case OpCodeOperandKind.r8_opcode:
				case OpCodeOperandKind.r16_reg:
				case OpCodeOperandKind.r16_rm:
				case OpCodeOperandKind.r16_opcode:
				case OpCodeOperandKind.seg_reg:
				case OpCodeOperandKind.k_reg:
				case OpCodeOperandKind.kp1_reg:
				case OpCodeOperandKind.k_rm:
				case OpCodeOperandKind.k_vvvv:
				case OpCodeOperandKind.mm_reg:
				case OpCodeOperandKind.mm_rm:
				case OpCodeOperandKind.xmm_reg:
				case OpCodeOperandKind.xmm_rm:
				case OpCodeOperandKind.xmm_vvvv:
				case OpCodeOperandKind.xmmp3_vvvv:
				case OpCodeOperandKind.xmm_is4:
				case OpCodeOperandKind.xmm_is5:
				case OpCodeOperandKind.ymm_reg:
				case OpCodeOperandKind.ymm_rm:
				case OpCodeOperandKind.ymm_vvvv:
				case OpCodeOperandKind.ymm_is4:
				case OpCodeOperandKind.ymm_is5:
				case OpCodeOperandKind.zmm_reg:
				case OpCodeOperandKind.zmm_rm:
				case OpCodeOperandKind.zmm_vvvv:
				case OpCodeOperandKind.zmmp3_vvvv:
				case OpCodeOperandKind.cr_reg:
				case OpCodeOperandKind.dr_reg:
				case OpCodeOperandKind.tr_reg:
				case OpCodeOperandKind.es:
				case OpCodeOperandKind.cs:
				case OpCodeOperandKind.ss:
				case OpCodeOperandKind.ds:
				case OpCodeOperandKind.fs:
				case OpCodeOperandKind.gs:
				case OpCodeOperandKind.al:
				case OpCodeOperandKind.cl:
				case OpCodeOperandKind.ax:
				case OpCodeOperandKind.dx:
				case OpCodeOperandKind.eax:
				case OpCodeOperandKind.rax:
				case OpCodeOperandKind.sti_opcode:
				case OpCodeOperandKind.imm2_m2z:
				case OpCodeOperandKind.imm8:
				case OpCodeOperandKind.imm8_const_1:
				case OpCodeOperandKind.imm8sex16:
				case OpCodeOperandKind.imm8sex32:
				case OpCodeOperandKind.imm8sex64:
				case OpCodeOperandKind.imm16:
				case OpCodeOperandKind.imm32:
				case OpCodeOperandKind.imm32sex64:
				case OpCodeOperandKind.imm64:
				case OpCodeOperandKind.seg_rDI:
				case OpCodeOperandKind.br16_1:
				case OpCodeOperandKind.br32_1:
				case OpCodeOperandKind.br64_1:
				case OpCodeOperandKind.br16_2:
				case OpCodeOperandKind.br32_4:
				case OpCodeOperandKind.br64_4:
				case OpCodeOperandKind.xbegin_2:
				case OpCodeOperandKind.xbegin_4:
				case OpCodeOperandKind.brdisp_2:
				case OpCodeOperandKind.brdisp_4:
					break;

				case OpCodeOperandKind.seg_rSI:
				case OpCodeOperandKind.es_rDI:
				case OpCodeOperandKind.seg_rBX_al:
					// string instructions, xlat
					opCount = 0;
					break;

				default:
					throw new InvalidOperationException();
				}
			}
		}

		MemorySize GetMemorySize(bool isBroadcast) {
			int index = (int)opCode.Code;
			if (isBroadcast)
				index += DecoderConstants.NumberOfCodeValues;
			return (MemorySize)InstructionMemorySizes.Sizes[index];
		}

		public string Format() {
			if (!opCode.IsInstruction) {
				switch (opCode.Code) {
				case Code.INVALID:		return "<invalid>";
				case Code.DeclareByte:	return "<db>";
				case Code.DeclareWord:	return "<dw>";
				case Code.DeclareDword:	return "<dd>";
				case Code.DeclareQword:	return "<dq>";
				default:				throw new InvalidOperationException();
				}
			}

			sb.Length = 0;

			Write(GetMnemonic(), upper: true);
			if (startOpIndex < opCount) {
				sb.Append(' ');
				int saeErIndex = opCount - 1;
				if (opCode.GetOpKind(saeErIndex) == OpCodeOperandKind.imm8)
					saeErIndex--;
				bool addComma = false;
				for (int i = startOpIndex; i < opCount; i++) {
					if (addComma)
						WriteOpSeparator();
					addComma = true;

					var opKind = opCode.GetOpKind(i);
					switch (opKind) {
					case OpCodeOperandKind.farbr2_2:
						sb.Append("ptr16:16");
						break;

					case OpCodeOperandKind.farbr4_2:
						sb.Append("ptr16:32");
						break;

					case OpCodeOperandKind.mem_offs:
						sb.Append("moffs");
						WriteMemorySize(GetMemorySize(isBroadcast: false));
						break;

					case OpCodeOperandKind.mem:
						WriteMemory();
						break;

					case OpCodeOperandKind.mem_mpx:
						if (opCode.Code == Code.Bndldx_bnd_mib || opCode.Code == Code.Bndstx_mib_bnd)
							sb.Append("mib");
						else
							WriteMemory();
						break;

					case OpCodeOperandKind.mem_vsib32x:
						sb.Append("vm32x");
						break;

					case OpCodeOperandKind.mem_vsib64x:
						sb.Append("vm64x");
						break;

					case OpCodeOperandKind.mem_vsib32y:
						sb.Append("vm32y");
						break;

					case OpCodeOperandKind.mem_vsib64y:
						sb.Append("vm64y");
						break;

					case OpCodeOperandKind.mem_vsib32z:
						sb.Append("vm32z");
						break;

					case OpCodeOperandKind.mem_vsib64z:
						sb.Append("vm64z");
						break;

					case OpCodeOperandKind.r8_or_mem:
						WriteGprMem(8);
						break;

					case OpCodeOperandKind.r16_or_mem:
						WriteGprMem(16);
						break;

					case OpCodeOperandKind.r32_or_mem:
					case OpCodeOperandKind.r32_or_mem_mpx:
						WriteGprMem(32);
						break;

					case OpCodeOperandKind.r64_or_mem:
					case OpCodeOperandKind.r64_or_mem_mpx:
						WriteGprMem(64);
						break;

					case OpCodeOperandKind.mm_or_mem:
						WriteRegMem("mm", GetVecIndex());
						break;

					case OpCodeOperandKind.xmm_or_mem:
						WriteRegMem("xmm", GetVecIndex());
						break;

					case OpCodeOperandKind.ymm_or_mem:
						WriteRegMem("ymm", GetVecIndex());
						break;

					case OpCodeOperandKind.zmm_or_mem:
						WriteRegMem("zmm", GetVecIndex());
						break;

					case OpCodeOperandKind.bnd_or_mem_mpx:
						WriteRegOp("bnd", GetBndIndex());
						sb.Append('/');
						WriteMemory();
						break;

					case OpCodeOperandKind.k_or_mem:
						WriteRegMem("k", GetKIndex());
						break;

					case OpCodeOperandKind.r8_reg:
					case OpCodeOperandKind.r8_opcode:
						WriteRegOp("r8");
						break;

					case OpCodeOperandKind.r16_reg:
					case OpCodeOperandKind.r16_rm:
					case OpCodeOperandKind.r16_opcode:
						WriteRegOp("r16");
						break;

					case OpCodeOperandKind.r32_reg:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r32_opcode:
					case OpCodeOperandKind.r32_vvvv:
						WriteRegOp("r32");
						AppendGprSuffix(r32_count, ref r32_index);
						break;

					case OpCodeOperandKind.r64_reg:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.r64_opcode:
					case OpCodeOperandKind.r64_vvvv:
						WriteRegOp("r64");
						AppendGprSuffix(r64_count, ref r64_index);
						break;

					case OpCodeOperandKind.seg_reg:
						sb.Append("Sreg");
						break;

					case OpCodeOperandKind.k_reg:
					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.k_vvvv:
						WriteRegOp("k", GetKIndex());
						break;

					case OpCodeOperandKind.kp1_reg:
						WriteRegOp("k", GetKIndex());
						sb.Append("+1");
						break;

					case OpCodeOperandKind.mm_reg:
					case OpCodeOperandKind.mm_rm:
						WriteRegOp("mm", GetVecIndex());
						break;

					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.xmm_vvvv:
					case OpCodeOperandKind.xmm_is4:
					case OpCodeOperandKind.xmm_is5:
						WriteRegOp("xmm", GetVecIndex());
						break;

					case OpCodeOperandKind.xmmp3_vvvv:
						WriteRegOp("xmm", GetVecIndex());
						sb.Append("+3");
						break;

					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.ymm_vvvv:
					case OpCodeOperandKind.ymm_is4:
					case OpCodeOperandKind.ymm_is5:
						WriteRegOp("ymm", GetVecIndex());
						break;

					case OpCodeOperandKind.zmm_reg:
					case OpCodeOperandKind.zmm_rm:
					case OpCodeOperandKind.zmm_vvvv:
						WriteRegOp("zmm", GetVecIndex());
						break;

					case OpCodeOperandKind.zmmp3_vvvv:
						WriteRegOp("zmm", GetVecIndex());
						sb.Append("+3");
						break;

					case OpCodeOperandKind.bnd_reg:
						WriteRegOp("bnd", GetBndIndex());
						break;

					case OpCodeOperandKind.cr_reg:
						WriteRegOp("cr");
						break;

					case OpCodeOperandKind.dr_reg:
						WriteRegOp("dr");
						break;

					case OpCodeOperandKind.tr_reg:
						WriteRegOp("tr");
						break;

					case OpCodeOperandKind.es:
						WriteRegister("es");
						break;

					case OpCodeOperandKind.cs:
						WriteRegister("cs");
						break;

					case OpCodeOperandKind.ss:
						WriteRegister("ss");
						break;

					case OpCodeOperandKind.ds:
						WriteRegister("ds");
						break;

					case OpCodeOperandKind.fs:
						WriteRegister("fs");
						break;

					case OpCodeOperandKind.gs:
						WriteRegister("gs");
						break;

					case OpCodeOperandKind.al:
						WriteRegister("al");
						break;

					case OpCodeOperandKind.cl:
						WriteRegister("cl");
						break;

					case OpCodeOperandKind.ax:
						WriteRegister("ax");
						break;

					case OpCodeOperandKind.dx:
						WriteRegister("dx");
						break;

					case OpCodeOperandKind.eax:
						WriteRegister("eax");
						break;

					case OpCodeOperandKind.rax:
						WriteRegister("rax");
						break;

					case OpCodeOperandKind.st0:
					case OpCodeOperandKind.sti_opcode:
						WriteRegister("ST");
						if (i == 0 && (opCode.Code == Code.Fcomi_st0_sti || opCode.Code == Code.Fcomip_st0_sti || opCode.Code == Code.Fucomi_st0_sti || opCode.Code == Code.Fucomip_st0_sti)) {
							// nothing, it should be ST and not ST(0)
						}
						else if (opKind == OpCodeOperandKind.st0)
							sb.Append("(0)");
						else {
							Debug.Assert(opKind == OpCodeOperandKind.sti_opcode);
							sb.Append("(i)");
						}
						break;

					case OpCodeOperandKind.imm2_m2z:
						sb.Append("imm2");
						break;

					case OpCodeOperandKind.imm8:
					case OpCodeOperandKind.imm8sex16:
					case OpCodeOperandKind.imm8sex32:
					case OpCodeOperandKind.imm8sex64:
						sb.Append("imm8");
						break;

					case OpCodeOperandKind.imm8_const_1:
						sb.Append("1");
						break;

					case OpCodeOperandKind.imm16:
						sb.Append("imm16");
						break;

					case OpCodeOperandKind.imm32:
					case OpCodeOperandKind.imm32sex64:
						sb.Append("imm32");
						break;

					case OpCodeOperandKind.imm64:
						sb.Append("imm64");
						break;

					case OpCodeOperandKind.seg_rSI:
					case OpCodeOperandKind.es_rDI:
					case OpCodeOperandKind.seg_rDI:
					case OpCodeOperandKind.seg_rBX_al:
						addComma = false;
						break;

					case OpCodeOperandKind.br16_1:
					case OpCodeOperandKind.br32_1:
					case OpCodeOperandKind.br64_1:
						sb.Append("rel8");
						break;

					case OpCodeOperandKind.br16_2:
					case OpCodeOperandKind.xbegin_2:
						sb.Append("rel16");
						break;

					case OpCodeOperandKind.br32_4:
					case OpCodeOperandKind.br64_4:
					case OpCodeOperandKind.xbegin_4:
						sb.Append("rel32");
						break;

					case OpCodeOperandKind.brdisp_2:
						sb.Append("disp16");
						break;

					case OpCodeOperandKind.brdisp_4:
						sb.Append("disp32");
						break;

					case OpCodeOperandKind.None:
					default:
						throw new InvalidOperationException();
					}

					if (i == 0) {
						if (opCode.CanUseOpMaskRegister) {
							sb.Append(' ');
							WriteRegDecorator("k", GetKIndex());
							if (opCode.CanUseZeroingMasking)
								WriteDecorator("z");
						}
					}
					if (i == saeErIndex) {
						if (opCode.CanSuppressAllExceptions)
							WriteDecorator("sae");
						if (opCode.CanUseRoundingControl) {
							if (opCode.Code != Code.EVEX_Vcvtusi2sd_xmm_xmm_rm32_er && opCode.Code != Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32_er)
								WriteDecorator("er");
						}
					}
				}
			}

			switch (opCode.Code) {
			case Code.Blendvpd_xmm_xmmm128:
			case Code.Blendvps_xmm_xmmm128:
			case Code.Pblendvb_xmm_xmmm128:
			case Code.Sha256rnds2_xmm_xmmm128:
				WriteOpSeparator();
				Write("<XMM0>", upper: true);
				break;

			case Code.Tpause_r32:
			case Code.Tpause_r64:
			case Code.Umwait_r32:
			case Code.Umwait_r64:
				WriteOpSeparator();
				Write("<edx>", upper: false);
				WriteOpSeparator();
				Write("<eax>", upper: false);
				break;
			}

			return sb.ToString();
		}

		void WriteMemorySize(MemorySize memorySize) {
			switch (opCode.Code) {
			case Code.Fldcw_m2byte:
			case Code.Fnstcw_m2byte:
			case Code.Fstcw_m2byte:
			case Code.Fnstsw_m2byte:
			case Code.Fstsw_m2byte:
				sb.Append("2byte");
				return;
			}

			switch (memorySize) {
			case MemorySize.Bound16_WordWord:
				sb.Append("16&16");
				break;

			case MemorySize.Bound32_DwordDword:
				sb.Append("32&32");
				break;

			case MemorySize.FpuEnv14:
				sb.Append("14byte");
				break;

			case MemorySize.FpuEnv28:
				sb.Append("28byte");
				break;

			case MemorySize.FpuState94:
				sb.Append("94byte");
				break;

			case MemorySize.FpuState108:
				sb.Append("108byte");
				break;

			case MemorySize.Fxsave_512Byte:
			case MemorySize.Fxsave64_512Byte:
				sb.Append("512byte");
				break;

			case MemorySize.Xsave:
			case MemorySize.Xsave64:
				// 'm' has already been appended
				sb.Append("em");
				break;

			case MemorySize.SegPtr16:
				sb.Append("16:16");
				break;

			case MemorySize.SegPtr32:
				sb.Append("16:32");
				break;

			case MemorySize.SegPtr64:
				sb.Append("16:64");
				break;

			case MemorySize.Fword6:
				if (!IsSgdtOrSidt())
					sb.Append("16&32");
				break;

			case MemorySize.Fword10:
				if (!IsSgdtOrSidt())
					sb.Append("16&64");
				break;

			default:
				int memSize = memorySize.GetSize();
				if (memSize != 0)
					sb.Append(memSize * 8);
				break;
			}

			if (IsFpuInstruction(opCode.Code)) {
				switch (memorySize) {
				case MemorySize.Int16:
				case MemorySize.Int32:
				case MemorySize.Int64:
					sb.Append("int");
					break;

				case MemorySize.Float32:
				case MemorySize.Float64:
				case MemorySize.Float80:
					sb.Append("fp");
					break;

				case MemorySize.Bcd:
					sb.Append("bcd");
					break;
				}
			}
		}

		bool IsSgdtOrSidt() {
			switch (opCode.Code) {
			case Code.Sgdt_m_16:
			case Code.Sgdt_m_32:
			case Code.Sgdt_m_64:
			case Code.Sidt_m_16:
			case Code.Sidt_m_32:
			case Code.Sidt_m_64:
				return true;
			}
			return false;
		}

		void WriteRegister(string register) => Write(register, upper: true);
		void WriteRegOp(string register) => Write(register, upper: false);
		void WriteRegOp(string register, int index) {
			WriteRegOp(register);
			if (index > 0)
				sb.Append(index);
		}
		void WriteDecorator(string decorator) {
			sb.Append('{');
			Write(decorator, upper: false);
			sb.Append('}');
		}
		void WriteRegDecorator(string register, int index) {
			sb.Append('{');
			Write(register, upper: false);
			sb.Append(index);
			sb.Append('}');
		}

		void AppendGprSuffix(int count, ref int index) {
			if (count <= 1)
				return;
			sb.Append((char)('a' + index));
			index++;
		}

		void WriteOpSeparator() => sb.Append(", ");

		void Write(string s, bool upper) {
			for (int i = 0; i < s.Length; i++) {
				var c = s[i];
				c = upper ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c);
				sb.Append(c);
			}
		}

		void WriteGprMem(int regSize) {
			Debug.Assert(!opCode.CanBroadcast);
			sb.Append('r');
			int memSize = GetMemorySize(isBroadcast: false).GetSize() * 8;
			if (memSize != regSize)
				sb.Append(regSize);
			sb.Append('/');
			WriteMemory();
		}

		void WriteRegMem(string register, int index) {
			WriteRegOp(register, index);
			sb.Append('/');
			WriteMemory();
		}

		void WriteMemory() {
			WriteMemory(isBroadcast: false);
			if (opCode.CanBroadcast) {
				sb.Append('/');
				WriteMemory(isBroadcast: true);
			}
		}

		void WriteMemory(bool isBroadcast) {
			var memorySize = GetMemorySize(isBroadcast);
			sb.Append('m');
			WriteMemorySize(memorySize);
			if (isBroadcast)
				sb.Append("bcst");
		}

		string GetMnemonic() {
			var code = opCode.Code;
			var mnemonic = code.ToMnemonic();
			switch (code) {
			case Code.Retfw:
			case Code.Retfw_imm16:
			case Code.Retfd:
			case Code.Retfd_imm16:
			case Code.Retfq:
			case Code.Retfq_imm16:
				mnemonic = Mnemonic.Ret;
				goto default;

			case Code.Iretd:
			case Code.Iretq:
			case Code.Pushad:
			case Code.Popad:
			case Code.Pushfd:
			case Code.Pushfq:
			case Code.Popfd:
			case Code.Popfq:
			case Code.Int3:
				return code.ToString();

			default:
				return mnemonic.ToString();
			}
		}

		static bool IsFpuInstruction(Code code) =>
			(uint)(code - Code.Fadd_m32fp) <= (uint)(Code.Fcomip_st0_sti - Code.Fadd_m32fp);
	}
}
#endif
