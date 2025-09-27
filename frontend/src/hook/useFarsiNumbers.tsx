import { isValidElement, cloneElement, Fragment } from "react"
import { toFarsiNumber } from "@/lib/locale"
function transformChildren(children: React.ReactNode): React.ReactNode {
    if (typeof children === "string" || typeof children == "number") {
        return toFarsiNumber(children.toString())
    }
    if (Array.isArray(children)) {
        return children.map((child, i) => <Fragment key={i}>{transformChildren(child)}</Fragment>)
    }
    if (isValidElement(children)) {
        const element = children as React.ReactElement<{ children?: React.ReactNode }>
        return cloneElement(element, {
            ...element.props,
            children: transformChildren(element.props.children),
        })
    }
    return children
}
export function useFarsiNumbers(children: React.ReactNode): React.ReactNode {
    return transformChildren(children)
}