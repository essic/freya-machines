﻿module Freya.Machines

open Aether
open Aether.Operators
open Freya.Core
open Freya.Core.Operators
open Hephaestus

(* Types

   Types representing some core concepts within the design and configuration
   of a conceptual Machine for the Freya system.

   The concept of a Decision maps the core Hephaestus abstraction of a
   decision of left or right to Freya, using simple boolean true or false to
   represent branching points (left or right are always intended to be an
   abstraction which should be targeted by a domain specific abstraction.

   The concept of Settings represents an extensible container for settings of
   any kind, which may be populated by any relevant component within the system
   to provide a source of setting information which cannot be known at design
   time.

   Optics are provided for these core Machine types. *)

type Decision =
    | Function of Freya<bool>
    | Literal of bool

    static member function_ =
        (function | Function f -> Some f
                  | _ -> None), (Function)

    static member literal_ =
        (function | Literal l -> Some l
                  | _ -> None), (Literal)

type Configuration =
    | Configuration of Map<string,obj>

    static member configuration_ =
        (fun (Configuration x) -> x), (Configuration)

    static member empty =
        Configuration (Map.empty)

(* Decisions

   Simple mapping from a Freya Decision to a stateful Hephaestus Decision,
   parameterized by the Freya State type. *)

[<RequireQualifiedAccess>]
[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Decision =

    type private Decision =
        Hephaestus.Decision<State>

    let private convert =
        function | true -> Right
                 | _ -> Left

    let map =
        function | Literal l -> Decision.Literal (convert l)
                 | Function f -> Decision.Function (convert <!> f)

(* Settings

   Functions for working with the Settings type, particularly using lenses to
   drive access and modification, providing "safe" ways to access indeterminate
   properties of the system at runtime. *)

[<RequireQualifiedAccess>]
[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Configuration =

    (* Optics *)

    let private default_ def =
        (function | Some a -> a
                  | _ -> def), (Some)

    let element_<'a> element def =
            Lens.ofIsomorphism Configuration.configuration_
        >-> Map.value_ element
        >-> Option.mapIsomorphism box_<'a>
        >-> default_ def

(* Patterns

   Commonly useful active recognizers for working with lens based access to
   data structures, particularly useful here for making access to configuration
   more concise. *)

let inline (|Get|) lens =
    Optic.get lens

let inline (|TryGet|_|) lens =
    Optic.get lens