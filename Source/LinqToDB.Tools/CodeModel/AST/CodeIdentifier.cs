﻿using System;
using System.Diagnostics;

namespace LinqToDB.CodeModel;

/// <summary>
/// Reference to identifier value. Used instead of string to allow identifier mutation in existing AST
/// (e.g. because initial value is not valid in target language or conflicts with existing identifiers).
/// </summary>
[DebuggerDisplay("{Name}")]
public sealed class CodeIdentifier : ICodeElement
{
	private string _name;

	public CodeIdentifier(string name, bool immutable)
	{
		_name     = name;
		Immutable = immutable;
	}

	public CodeIdentifier(string name, NameFixOptions? fixOptions, int? position)
	{
		_name      = name;
		FixOptions = fixOptions;
		Position   = position;
	}

	/// <summary>
	/// When <c>true</c>, is it not allowed to change identifier name.
	/// Should be used only for identifiers that define name of external object (e.g. class, method or property)
	/// as renaming such identifier will lead to wrong generated code with reference to unknown object.
	/// Setting it for objects, generated by code generator is not recommended as it will prevent it from
	/// renaming on naming conflicts and could result in incorrect generated code with duplicate/conflicting
	/// identifiers.
	/// </summary>
	public bool            Immutable { get; }

	/// <summary>
	/// Identifier value.
	/// </summary>
	public string          Name
	{
		get => _name;
		internal set
		{
			if (Immutable)
				throw new InvalidOperationException($"Immutable identifier rename attempt detected: {_name} => {value}");

			_name = value;
		}
	}
	/// <summary>
	/// Optional normalization hits for invalid identifier normalization logic.
	/// </summary>
	public NameFixOptions? FixOptions { get; }
	/// <summary>
	/// Optional identifier ordinal for identifier normalizer (e.g. see <see cref="NameFixType.SuffixWithPosition"/>).
	/// </summary>
	public int?            Position   { get; }

	CodeElementType ICodeElement.ElementType => CodeElementType.Identifier;
}
