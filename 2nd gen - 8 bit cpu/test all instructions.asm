#defmem	0x0ff		0x30	// this address should be not greater than 255 (or even 63), because the breadboard test circuit has a limited address space
#defmem	0x0ef		0x11

				// load values to all registers
				LD A, 1
				LD B, 2
				LD H, 3
				LD L, 4

				LD C, 32
				LD D, 64
				LD E, 128
				LD F, 255

				// copy from one register to another
				LD A, B		// from bank A to bank A (A = 2)
				LD D, E		// from bank B to bank B (D = 128)
				LD A, F		// from bank A to bank B (A = 255)
				LD C, H		// from bank B to bank A (C = 3)

loop:			
				LD H, 0x80
				SHR H		// Shift Right (divide by 2), not a 74181 function, it's implemented by a 74HCT244
				JP Z, :exit_loop
				JP :loop

exit_loop:		
				LD A, 255
				INC A
				JP C, :c_flag_ok
				JP Z, :c_flag_ok	// just in case the JP C doesn't work
				JP :exit_loop

c_flag_ok:		
				// test ALU ops
				LD L, 63
				LD F, 13		// F = 0b00001101
				SUB L, F		// L = 50
				DEC L			// L = 49 (0b00110001)
				NOT L			// L = 0b11001110
				OR L, F			// L = 0b11001111

				// test get/store values from/to memory
				LD A, [0x0ff]		// A = 0x30
				ST [0x0fe], A
				LD H, [0x0fe]		// H = 0x30

				LD H, 0x00
				LD L, 0xef
				LD A, [HL]			// A = 0x11
				ST [HL], A
				LD B, [HL]			// B = 0x11

				// test CALL instr
				LD H, 15
				LD F, 1
				CALL :sub_add_h_f	// H = 16

sub_add_h_f:	
				ADD H, F
				RET
