# Your program's name
PROG := blink-comparator

# List of source files
SRCS := $(wildcard *.go)

# Output directory
OUT_DIR := ./bin

# 32-bit ARMv7 (Raspberry Pi 2, Raspberry Pi 3 in 32-bit mode)
ARMV7L := GOOS=linux GOARCH=arm GOARM=7

# 64-bit ARM (Raspberry Pi 3, Raspberry Pi 4)
ARMV8 := GOOS=linux GOARCH=arm64

# 64-bit x86 (most modern PCs)
AMD64 := GOOS=linux GOARCH=amd64

# Remote machine
REMOTE := kyle@ganymede:/home/kyle/$(PROG)

.PHONY: all
all: $(OUT_DIR)/armv7l/$(PROG) $(OUT_DIR)/armv8/$(PROG) $(OUT_DIR)/amd64/$(PROG)

$(OUT_DIR)/armv7l/$(PROG): $(SRCS)
	mkdir -p $(OUT_DIR)/armv7l
	$(ARMV7L) go build -o $@ $^

$(OUT_DIR)/armv8/$(PROG): $(SRCS)
	mkdir -p $(OUT_DIR)/armv8
	$(ARMV8) go build -o $@ $^

$(OUT_DIR)/amd64/$(PROG): $(SRCS)
	mkdir -p $(OUT_DIR)/amd64
	$(AMD64) go build -o $@ $^

.PHONY: install
install: $(OUT_DIR)/armv7l/$(PROG)
	scp $< $(REMOTE)

.PHONY: clean
clean:
	rm -rf $(OUT_DIR)/armv7l $(OUT_DIR)/armv8 $(OUT_DIR)/amd64
