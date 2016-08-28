# Base configuration for building Chip8.
cmake_minimum_required(VERSION 3.6)
project(Chip8)

# Include library headers.
include_directories("lib/di/include")
include_directories("lib/sfml/include")

# Include our own headers.
include_directories("include")
include_directories("include/Chip8")

# Set C++ compiler flags. As of now, they're made with Clang in mind.
set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++14 -Weverything -Wno-c++98-compat -Wno-c++98-compat-pedantic -Wno-unused-macros -Wno-newline-eof -Wno-exit-time-destructors -Wno-global-constructors -Wno-gnu-zero-variadic-macro-arguments -Wno-documentation -Wno-shadow -Wno-missing-prototypes")

# Add source files and compile.
set(SOURCE_FILES
    src/Chip8/Display.cpp
    src/Chip8/Emulator.cpp
    src/Chip8/EmulatorSFML.cpp
    src/Chip8/State.cpp
    src/Chip8/Storage.cpp
    src/Launcher/main.cpp)

add_executable(Chip8 ${SOURCE_FILES})

# Add header files as custom target so they show up in the project file structure in the IDE.
FILE(GLOB_RECURSE HEADER_FILES "include/*.hpp")
add_custom_target(headers SOURCES ${HEADER_FILES})
