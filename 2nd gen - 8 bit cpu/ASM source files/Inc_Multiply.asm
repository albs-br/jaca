// Multiply A x B, result in A (8 bits, max value 255)
// Destroys A, B, D

#org	0xd00

multiply:
	LD D, A
	LD A, 0x0
	DNW B
mu_loop:
	JP Z, :mu_end
	ADD A, D
	DEC B
	JP :mu_loop
mu_end:
	RET
