project('SE_PLab18', 'cpp',
        version : '1.0.0',
        default_options : ['warning_level=3', 'cpp_std=c++20'])

sources = files(
    'main.cpp',
    'Modules/Source/Error.cpp',
    'Modules/Source/Log.cpp',
    'Modules/Source/Parm.cpp',
    'Modules/Source/In.cpp',
    'Modules/Source/Out.cpp',
    'Modules/Source/LT.cpp',
    'Modules/Source/IT.cpp',
    'Modules/Source/Lexer.cpp',
    'Modules/Source/FST.cpp',
    'Modules/Source/Mfst.cpp',
    'Modules/Source/GRB.cpp'
)

iconv_dep = dependency('iconv', required : true)

SE_PLab18 = executable('SE_PLab18',
                       sources,
                       include_directories : include_directories('Modules/Headers'),
                       dependencies : [iconv_dep],
                       install : true)