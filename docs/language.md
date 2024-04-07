# Language

The output of the language is always a _File_

_File_ --> _Block_+

_Block_ --> CSharp | ::_directive_; | ::_variable_insertion_::

_directive_ -> (_filename_ | _for-i_)

_filename_ -> filename _identifier_

_for-i_ -> 
    (for _range_ | for _range_ as _identifier_);
    _Block_ 
    ::endfor
_range_ -> __[0-9]+__..__[0-9]+__

_identifier_ -> __[A-Za-z][A-Za-z0-9]*__

_variable_insertion_ -> _identifier_