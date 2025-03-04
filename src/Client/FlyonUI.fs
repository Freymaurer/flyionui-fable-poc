module FlyonUI

open Fable.Core
open Fable.Core.JsInterop
open Feliz

module FlyonUIHelper =

    // Define the external FlyonUI methods
    [<Global>]
    type IStaticMethods =
        abstract member autoInit: unit -> unit

    [<Global>]
    let HSStaticMethods: IStaticMethods = jsNative

    // Load FlyonUI dynamically
    let loadFlyonui () = promise {
        let! _ = importDynamic "flyonui/flyonui"
        HSStaticMethods.autoInit ()
    }

[<RequireQualifiedAccess>]
type FlyonUI =
    static member load(?dependencies: obj[]) =
        let dependencies = defaultArg dependencies [||]
        React.useEffect ((fun () -> FlyonUIHelper.loadFlyonui () |> Promise.start), dependencies)

type SortableData = {
    item: Browser.Types.HTMLElement
    ``to``: Browser.Types.HTMLElement // target list
    from: Browser.Types.HTMLElement // previous list
    oldIndex: int // element's old index within old parent
    newIndex: int // element's new index within new parent
    oldDraggableIndex: int // element's old index within old parent, only counting draggable elements
    newDraggableIndex: int // element's new index within new parent, only counting draggable elements
    clone: Browser.Types.HTMLElement // the clone element
    pullMode: string
}

type OnMoveData = {
    dragged: Browser.Types.HTMLElement // dragged HTMLElement
    draggedRect: Browser.Types.ClientRect // DOMRect {left, top, right, bottom}
    related: Browser.Types.HTMLElement // HTMLElement on which have guided
    relatedRect: Browser.Types.ClientRect // DOMRect
    willInsertAfter: bool // Boolean that is true if Sortable will insert drag element after target by default
}

type OldIndexData = { oldIndex: int }

type DataTransferData = { setData: string * string -> unit }

type DragElData = { textContent: string }

type OriginalData = { clientY: string }

type OnCloneData = {
    origEl: Browser.Types.HTMLElement
    cloneEl: Browser.Types.HTMLElement
}

type OnFilterData = { itemEl: Browser.Types.HTMLElement }

type OnChangeData = { newIndex: int }

module Sortable =

    [<RequireQualifiedAccess>]
    type Interop =
        static member inline mkProperty<'ControlProperty> (key: string) (value: obj) : 'ControlProperty =
            unbox (key, value)

    [<Erase>]
    type ISortableProp = interface end

    type ISortable =
        abstract member create: Browser.Types.HTMLElement -> obj -> unit

    [<ImportDefault("sortablejs")>]
    let Sortable: ISortable = jsNative

    type sortable =
        static member inline group(value: string) =
            Interop.mkProperty<ISortableProp> "group" value

        /// ms, animation speed moving items when sorting, `0` — without animation
        static member inline animation(value: int) =
            Interop.mkProperty<ISortableProp> "animation" value

        /// sorting inside list
        static member inline sort(value: bool) =
            Interop.mkProperty<ISortableProp> "sort" value

        /// time in milliseconds to define when the sorting should start
        static member inline delay(value: int) =
            Interop.mkProperty<ISortableProp> "delay" value

        /// only delay if user is using touch
        static member inline delayOnTouchOnly(value: bool) =
            Interop.mkProperty<ISortableProp> "delayOnTouchOnly" value

        /// px, how many pixels the point should move before cancelling a delayed drag event
        static member inline touchStartThreshold(value: int) =
            Interop.mkProperty<ISortableProp> "delayOnTouchOnly" value

        /// Disables the sortable if set to true.
        static member inline disabled(value: bool) =
            Interop.mkProperty<ISortableProp> "disabled" value

        /// Easing for animation. Defaults to null. See https://easings.net/ for examples.
        /// "cubic-bezier(1, 0, 0, 1)"
        static member inline easing(value: string) =
            Interop.mkProperty<ISortableProp> "easing" value

        /// Drag handle selector within list items. ".my-handle"
        static member inline handle(value: string) =
            Interop.mkProperty<ISortableProp> "handle" value

        /// Selectors that do not lead to dragging (String or Function). ".ignore-elements"
        static member inline filter(value: string) =
            Interop.mkProperty<ISortableProp> "filter" value

        /// Call `event.preventDefault()` when triggered `filter`
        static member inline preventOnFilter(value: bool) =
            Interop.mkProperty<ISortableProp> "preventOnFilter" value

        /// Specifies which items inside the element should be draggable
        static member inline draggable(value: Browser.Types.HTMLElement) =
            Interop.mkProperty<ISortableProp> "draggable" value

        /// HTML attribute that is used by the `toArray()` method
        static member inline dataIdAttr(value: string) =
            Interop.mkProperty<ISortableProp> "dataIdAttr" value

        /// Class name for the drop placeholder
        static member inline ghostClass(value: string) =
            Interop.mkProperty<ISortableProp> "ghostClass" value

        /// Class name for the chosen item
        static member inline chosenClass(value: string) =
            Interop.mkProperty<ISortableProp> "chosenClass" value

        /// Class name for the dragging item
        static member inline dragClass(value: string) =
            Interop.mkProperty<ISortableProp> "dragClass" value

        /// Threshold of the swap zone
        static member inline swapThreshold(value: int) =
            Interop.mkProperty<ISortableProp> "swapThreshold" value

        /// Will always use inverted swap zone if set to true
        static member inline invertSwap(value: bool) =
            Interop.mkProperty<ISortableProp> "invertSwap" value

        /// Threshold of the inverted swap zone (will be set to swapThreshold value by default)
        static member inline invertedSwapThreshold(value: int) =
            Interop.mkProperty<ISortableProp> "invertedSwapThreshold" value

        // ignore the HTML5 DnD behaviour and force the fallback to kick in
        static member inline forceFallback(value: bool) =
            Interop.mkProperty<ISortableProp> "forceFallback" value

        /// Class name for the cloned DOM Element when using forceFallback
        static member inline fallbackClass(value: string) =
            Interop.mkProperty<ISortableProp> "fallbackClass" value

        /// Appends the cloned DOM Element into the Document's Body
        static member inline fallbackOnBody(value: bool) =
            Interop.mkProperty<ISortableProp> "fallbackOnBody" value

        /// Specify in pixels how far the mouse should move before it's considered as a drag.
        static member inline fallbackTolerance(value: int) =
            Interop.mkProperty<ISortableProp> "fallbackTolerance" value

        static member inline dragoverBubble(value: bool) =
            Interop.mkProperty<ISortableProp> "dragoverBubble" value

        /// Remove the clone element when it is not showing, rather than just hiding it
        static member inline removeCloneOnHide(value: bool) =
            Interop.mkProperty<ISortableProp> "removeCloneOnHide" value

        static member inline setData(value: DataTransferData -> DragElData -> unit) =
            Interop.mkProperty<ISortableProp> "setData" (System.Func<_, _, _> value)

        /// Element is chosen
        static member inline onChoose(value: OldIndexData -> unit) =
            Interop.mkProperty<ISortableProp> "onChoose" (System.Func<_, _> value)

        /// Element is unchosen
        static member inline onUnchoose(value: SortableData -> unit) =
            Interop.mkProperty<ISortableProp> "onUnchoose" (System.Func<_, _> value)

        /// Element dragging started
        static member inline onStart(value: OldIndexData -> unit) =
            Interop.mkProperty<ISortableProp> "onStart" (System.Func<_, _> value)

        /// Element dragging ended
        static member inline onEnd(value: SortableData -> unit) =
            Interop.mkProperty<ISortableProp> "onEnd" (System.Func<_, _> value)

        /// Element is dropped into the list from another list
        static member inline onAdd(value: SortableData -> unit) =
            Interop.mkProperty<ISortableProp> "onAdd" (System.Func<_, _> value)

        /// Changed sorting within list
        static member inline onUpdate(value: SortableData -> unit) =
            Interop.mkProperty<ISortableProp> "onUpdate" (System.Func<_, _> value)

        /// Called by any change to the list (add / update / remove)
        static member inline onSort(value: SortableData -> unit) =
            Interop.mkProperty<ISortableProp> "onSort" (System.Func<_, _> value)

        /// Element is removed from the list into another list
        static member inline onRemove(value: SortableData -> unit) =
            Interop.mkProperty<ISortableProp> "onRemove" (System.Func<_, _> value)

        /// Attempt to drag a filtered element
        static member inline onFilter(value: OnFilterData -> unit) =
            Interop.mkProperty<ISortableProp> "onFilter" (System.Func<_, _> value)

        /// Event when you move an item in the list or between lists
        static member inline onMove(value: OnMoveData -> unit) =
            Interop.mkProperty<ISortableProp> "onMove" (System.Func<_, _> value)

        /// Called when creating a clone of element
        static member inline onClone(value: OnCloneData -> unit) =
            Interop.mkProperty<ISortableProp> "onClone" (System.Func<_, _> value)

        /// Called when dragging element changes position
        static member inline onChange(value: OnChangeData -> unit) =
            Interop.mkProperty<ISortableProp> "onChange" (System.Func<_, _> value)

    module sortable =
        /// Direction of Sortable (will be detected automatically if not given)
        [<Erase>]
        type direction =
            static member inline horizontal = Interop.mkProperty<ISortableProp> "direction" "horizontal"
            static member inline vertical = Interop.mkProperty<ISortableProp> "direction" "vertical"

module FlyonUI =
    type DragAndDrop =
        static member create (ref: IRefValue<Browser.Types.HTMLElement option>) (options: Sortable.ISortableProp list) =
            if ref.current.IsSome then
                Sortable.Sortable.create ref.current.Value (!!options |> createObj |> unbox)

open Sortable

[<Mangle(false); Erase>]
type flyonui =

    [<ReactComponent>]
    static member DragAndDrop() =

        let listRef = React.useElementRef ()

        let options = [
            sortable.animation 500
            sortable.dragClass "!border-0"
            sortable.onEnd (fun (d: SortableData) -> printfn "container id: %A" d.from.id)
            sortable.onChange (fun (d: OnChangeData) -> printfn "newIndex %A" d.newIndex)
        ]

        React.useLayoutEffectOnce (fun () -> FlyonUI.DragAndDrop.create listRef options)

        let listElement index =
            Html.li [
                prop.className "flex items-center gap-3"
                prop.style [ style.backgroundColor "lightBlue" ]
                prop.children [
                    Html.span [ prop.className "icon-[tabler--grip-vertical] cursor-move" ]
                    Html.span [
                        prop.className "text-base-content"
                        prop.text $"Item {index}"
                    ]
                ]
            ]

        Html.ul [
            prop.id "testing-id"
            prop.ref listRef
            prop.className "bg-base-100 text-base-content p-4 rounded divide-base-content/25 flex flex-col divide-y"
            prop.children [
                for i in 0..10 do
                    listElement i
            ]
        ]
