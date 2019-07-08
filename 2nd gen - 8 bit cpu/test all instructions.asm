#defmem	0x0ff		0x30	// this address should be not greater than 255 (or even 63)

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
				LD A, B		// from bank A to bank A (B = 1)
				LD D, E		// from bank B to bank B (D = 128)
				LD A, F		// from bank A to bank B (A = 255)
				LD C, H		// from bank B to bank A (C = 3)

loop:			
				SHL B
				JP Z, :exit_loop
				JP :loop

exit_loop:		
				LD A, 255
				LD C, 1
				ADD A, C
				JP C, :c_flag_ok

c_flag_ok:		
				LD H, 15
				LD F, 1
				CALL :sub_add_h_f	// H = 16

				// test ALU ops
				LD L, 63
				LD F, 13
				SUB L, F		// L = 50
				INC L			// L = 51 (0b00110011)
				NOT L			// L = 0b11001100
				OR L, F			// L = 0b11001101

				// test get/store values from/to memory
				LD A, [0x0ff]		// A = 0x30
				ST [0x0fe], A
				LD H, [0x0fe]		// H = 0x30

sub_add_h_f:	
				ADD H, F
				RET
