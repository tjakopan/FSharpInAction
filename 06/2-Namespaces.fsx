namespace Foo

type Order = { Name: string }

namespace Bar.Baz

open Foo

type Customer = { Name: string; LastOrder: Foo.Order }

open Foo
type CustomerTwo = { LastOrder: Order }
