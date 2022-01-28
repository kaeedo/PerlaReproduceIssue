module PerlaIssue.App

open Sutil
open Sutil.Program
open Sutil.DOM
open Sutil.Attr

open Fable.Etebase

let view  =
    let count = Store.make 0

    let isEtebaseServer = ObservablePromise<bool>()

    let getIsServer () =
        isEtebaseServer.Run
        <| promise {
            let! response = Account.account.isEtebaseServer("https://api.etebase.com/")

            return response
        }

    Html.div [
        onMount (fun _ -> getIsServer ()) [ Once ]
        disposeOnUnmount [
            count
        ]

        Html.div [
            text "Call to isValidServer should be true:"
        ]
        Html.div [
            Bind.el (
                isEtebaseServer,
                function
                | Waiting -> text "Waiting"
                | Error e -> text "Error"
                | Result r -> text (r.ToString())
            )
        ]


        Bind.el (
            count,
            fun c ->
                Html.h3 [
                    text <| c.ToString()
                ]
        )
        Html.div [
            Html.button [
                onClick (fun _ -> count |> Store.modify (fun c -> c - 1)) []
                text "-"
            ]

            Html.button [
                onClick (fun _ -> count |> Store.modify (fun c -> c + 1)) []
                text "+"
            ]
        ]
    ]

view  |> mountElement "app"
