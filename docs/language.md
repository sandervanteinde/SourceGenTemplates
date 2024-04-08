# Language

The output of the language is always a _File_

_File_ --> _Block_+

_Block_ --> CSharp | ::_directive_ | ::_variable_insertion_::

_directive_ -> (_filename_ | _for-i_ | _foreach_)

_filename_ -> filename _expression_;

_expression_ -> _identifier_ | _string_ | _number_

_string_ -> "__[^"\n\r]+__"

_number_ -> __-?[0-9]+__

_for-i_ ->
(for _range_ | for _range_ as _identifier_);
_Block_*
::end;

_foreach_ ->
((foreach _foreach-type_ in _foreach-target_) | (foreach _foreach-type_ in _foreach-target_ as _identifier_));
_Block_*
::end;

_foreach-type_ -> class

_foreach-target_ -> assembly

_range_ -> _number_.._number_

_identifier_ -> __[A-Za-z][A-Za-z0-9]*__

_variable_insertion_ -> _identifier_