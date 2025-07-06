# Hierarchy
## Bank
It is Head Bank
IT can have multiple Branches or Multiple outlets

## Branch
It is branch or bank.
It can have multiple Branch (Sub Branch, Sub-Sub Branch.. Go Nuts.) Or Multiple outlets

## Outlet
It is outlet. (I am thinking this as ATM or Cash Counter)
It can have multiple Outlet or Sub-Outlet or Sub-Sub-Outlet.. Go Nuts.

# Interaction and Access Controll
People may interact to system from Bank, Branch, Outlet.
People's data are scoped to where it is registered and all their parents. Folowing is the example node structure.
<pre><code>
Head Bank (1)
├── Main Branch (1.1)
│   ├── Sub Branch (1.1.1)
│   │   └── Sub-Sub Branch (1.1.1.1)
│   ├── Sub Branch (1.1.2)
│   ├── Outlet (Cash Counter) (1.1.3)
│   └── Outlet (ATM) (1.1.4)
├── Main Branch (1.2)
│   ├── Sub Branch (1.2.1)
│   │   └── Outlet (Cash Counter) (1.2.1.1)
│   └── Outlet (ATM Array) (1.2.2)
│       │── Outlet (ATM) (1.2.2.1)
│       └── Outlet (ATM) (1.2.2.2)
└── Direct Outlet (1.3)
    ├── Sub-Outlet 1
    └── Sub-Sub-Outlet 1.1
</code></pre>
_Customer registered in Sub Branch `1.2.1`'s Data is available to `1.2.1`, `1.2`, `1` only_<br>
_Transaction History from `1.2.1.1` is available to `1.2.1.1`, `1.2.1`, `1.2`, `1` only_<br>
_Parent has full access and controll over it's child's data._
 