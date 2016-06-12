﻿namespace Freya.Machines.Http.Cors

#nowarn "46"

open Freya.Machines.Http
open Freya.Machines.Http.Cors.Machine.Configuration
open Freya.Machines.Http.Cors.Machine.Components

(* Use *)

[<AutoOpen>]
module Use =

    let cors =
        set [ Cors.component ]

(* Syntax *)

[<AutoOpen>]
module Syntax =

    (* Extension *)

    type HttpMachineBuilder with

        [<CustomOperation ("corsEnabled", MaintainsVariableSpaceUsingBind = true)>]
        member inline __.CorsEnabled (m, a) =
            HttpMachine.set (m, Extension.enabled_, Decision.infer a)

    (* Properties *)

    type HttpMachineBuilder with

        [<CustomOperation ("corsOrigins", MaintainsVariableSpaceUsingBind = true)>]
        member inline __.CorsOrigins (m, a) =
            HttpMachine.set (m, Properties.Resource.origins_, Origins.infer a)

        [<CustomOperation ("corsMethods", MaintainsVariableSpaceUsingBind = true)>]
        member inline __.CorsMethods (m, a) =
            HttpMachine.set (m, Properties.Resource.methods_, Methods.infer a)