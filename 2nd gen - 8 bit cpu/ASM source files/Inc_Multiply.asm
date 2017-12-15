// multiply A x B, result in A

#org	0x100

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
