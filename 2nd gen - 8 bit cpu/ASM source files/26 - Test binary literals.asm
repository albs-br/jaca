// Test binary literals


	LD C, 0x8			// constant to refresh led matrix
	LD D, 0				// first line
	
init:

	LD A, 0b00000001	// bit pattern of pixel at extreme right
	
loop:
	SHL A
	JP Z, :init
	
	OUT 1, A, D
	out 1, a, c

	JP :loop
 