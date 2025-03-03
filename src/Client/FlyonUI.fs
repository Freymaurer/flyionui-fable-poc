module FlyonUI

open Fable.Core
open Fable.Core.JsInterop

module FlyonUIHelper =

    // Define the external FlyonUI methods
    [<Global>]
    type IStaticMethods =
        abstract member autoInit : unit -> unit

    [<Global>]
    let HSStaticMethods: IStaticMethods = jsNative

    // Load FlyonUI dynamically
    let loadFlyonui () =
        promise {
            let! _ = importDynamic "flyonui/flyonui"
            HSStaticMethods.autoInit()
        }

open Feliz


type React with
    static member useFlyon(?dependencies: obj []) =
        let dependencies = defaultArg dependencies [||]
        React.useEffect(
            (fun () -> FlyonUIHelper.loadFlyonui () |> Promise.start),
            dependencies
        )

type IAdvancedSelect =

    abstract member setValue: string -> unit

type AdvancedSelectTemplateOptions = {
    title: IReactProperty
    description: IReactProperty
    icon: IReactProperty
}

type advancedSelect =

    static member private DATA_TITLE = prop.custom("data-title", true)
    static member private DATA_DESCRIPTION = prop.custom("data-description", true)
    static member private DATA_ICON = prop.custom("data-icon", true)
    static member private TEMPLATE_VARS = { title = advancedSelect.DATA_TITLE; description = advancedSelect.DATA_DESCRIPTION; icon=advancedSelect.DATA_ICON}

    static member data_select (value: string) = prop.custom("data-select", value)
    static member data_select (value: #seq<string * obj>) =
        let pojo = createObj value
        let json = Fable.Core.JS.JSON.stringify pojo
        prop.custom("data-select", json)
    static member placeholder (v: string) = "placeholder" ==> v
    static member hasSearch = "hasSearch" ==> "true"

    static member toggleClasses (v: string) = "toggleClasses" ==> v
    static member toggleClasses (v: #seq<string>) = "toggleClasses" ==> (v |> String.concat " ")

    static member toggleTag (builder: AdvancedSelectTemplateOptions -> ReactElement) =
        let template =
            builder advancedSelect.TEMPLATE_VARS
            |> ReactDOMServer.renderToString
        "toggleTag" ==> template

    static member optionTemplate (builder: AdvancedSelectTemplateOptions -> ReactElement) =
        let template =
            builder advancedSelect.TEMPLATE_VARS
            |> ReactDOMServer.renderToString
        "optionTemplate" ==> template

    static member getInstance (element: Browser.Types.Element) : IAdvancedSelect =
        let instance = Browser.Dom.window?HSSelect?getInstance(element)
        instance

    static member getInstance (id: string) : IAdvancedSelect =
        Browser.Dom.window?HSSelect?getInstance(id)

type advancedSelectOption =
    static member data_select_option (value: string) = prop.custom("data-select-option", value)
    static member data_select_option (value: #seq<string * obj>) =
        let pojo = createObj value
        let json = Fable.Core.JS.JSON.stringify pojo
        prop.custom("data-select-option", json)

    static member description (v: string) = "description" ==> v

type contextMenu =
    /// Add this attribute to a component to spawn it on context menu.
    ///
    /// Use it with prop.className [contextMenu.isContextMenu] to make it work.
    static member isContextMenu = "[--trigger:contextmenu]"

[<Mangle(false); Erase>]
type flyonui =
    [<ReactComponent>]
    static member TestSelect () =
        let selectRef = React.useElementRef()
        let optionTemplate =
            fun (vars: AdvancedSelectTemplateOptions) ->
                Html.div [
                    prop.className "flex items-center gap-4"
                    prop.children [
                        Html.div [
                            prop.className "w-full"
                            prop.children [
                                Html.div [
                                    prop.className "font-semibold text-base-content"
                                    vars.title
                                ]
                                Html.div [
                                    prop.className "text-sm text-base-content/80 mt-1.5"
                                    vars.description
                                ]
                            ]
                        ]
                        Html.span [
                            prop.className "icon-[tabler--check] flex-shrink-0 size-4 text-primary hidden selected:block"
                        ]
                    ]
                ]
        Html.div [
            prop.className "w-full border flex p-6 m-2"
            prop.children [
                Html.div [
                    prop.className "max-w-md"
                    prop.children [
                        Html.select [
                            prop.ref selectRef
                            advancedSelect.data_select [
                                advancedSelect.placeholder "Select an option.. "
                                advancedSelect.hasSearch
                                advancedSelect.toggleTag (fun vars ->
                                    Html.button [
                                        prop.type'.button
                                        prop.children [
                                            Html.span [
                                                prop.className "text-base-content"
                                                vars.title
                                            ]
                                        ]
                                    ]
                                )
                                advancedSelect.toggleClasses "advance-select-toggle min-w-[400px]"
                                "dropdownClasses" ==> "advance-select-menu *:bg-base-100"
                                "optionClasses" ==> "advance-select-option selected:active"
                                advancedSelect.optionTemplate optionTemplate
                                "extraMarkup" ==> "<span class=\"icon-[tabler--caret-up-down] flex-shrink-0 size-4 text-base-content absolute top-1/2 end-3 -translate-y-1/2\"></span>"
                            ]
                            prop.className "hidden"
                            prop.children [
                                Html.option [
                                    prop.value ""
                                    prop.text "Choose"
                                ]
                                Html.option [
                                    prop.value "name"
                                    prop.text "Full Name"
                                    advancedSelectOption.data_select_option [
                                        advancedSelectOption.description "Full Name"
                                    ]
                                ]
                                Html.option [
                                    prop.value "email"
                                    prop.text "Email"
                                    advancedSelectOption.data_select_option [
                                        advancedSelectOption.description "Email Address"
                                    ]
                                ]
                                Html.option [
                                    prop.value "description"
                                    prop.text "Project Description"
                                    advancedSelectOption.data_select_option [
                                        advancedSelectOption.description "Project Description"
                                    ]
                                ]
                                Html.option [
                                    prop.value "user_id"
                                    prop.text "User Identification Number"
                                    advancedSelectOption.data_select_option [
                                        advancedSelectOption.description "User Identification Number"
                                    ]
                                ]
                            ]
                        ]

                        Html.button [
                            prop.className "btn btn-primary"
                            prop.onClick(fun _ ->
                                if selectRef.current.IsSome then
                                    let instance = advancedSelect.getInstance(selectRef.current.Value)
                                    instance.setValue("email")
                            )
                            prop.text "Set Email"
                        ]
                    ]
                ]
            ]
        ]

    static member ContextMenu() =
        Html.div [
            prop.className [
                contextMenu.isContextMenu
                "dropdown"
            ]
            prop.children [
                Html.div [
                    prop.className "dropdown-toggle w-96 h-25 flex justify-center items-center rounded-lg border-2 border-dashed border-primary shadow-sm bg-base-100 m-2 p-2"
                    prop.text "Right Click Me"
                ]
                Html.div [
                    prop.className "dropdown-menu dropdown-open:opacity-100 hidden min-w-60"
                    prop.children [
                        for i in 1..5 do
                            Html.div [
                                prop.className "py-2 px-4 cursor-pointer hover:bg-base-200"
                                prop.text $"Item {i}"
                            ]
                    ]
                ]
            ]
        ]