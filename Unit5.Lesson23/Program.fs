type Customer2 =
  { CustomerId: int
    Email: string
    Telephone: string
    Address: string }

let createCustomer2 customerId email telephone address =
  { CustomerId = customerId
    Email = email
    Telephone = telephone
    Address = address }

let customer2 =
  createCustomer2 1 "alextom@gmail.com" "0992723322" "Peremohy ave 123"

type Address2 = Address2 of string
let myAddress2 = Address2 "Peremohy ave 123"
let isTheSameAddress = (myAddress2 = "Peremohy ave 123")
let (Address2 addressData) = myAddress2 //unwrapping the Address into its raw string as addressData


type CustomerId = CustomerId of string

type ContactDetails =
  | Email of string
  | Telephone of string
  | Address of string

type Customer =
  { CustomerId: CustomerId
    PrimaryContactDetails: ContactDetails
    SecondaryContactDetails: ContactDetails option }

let createCustomer customerId primaryContact secondaryDetails =
  { CustomerId = customerId
    PrimaryContactDetails = primaryContact
    SecondaryContactDetails = secondaryDetails }

let customer =
  createCustomer (CustomerId "1") (Email "alextom@gmail.com") None

printfn "%A" customer

let customer2 =
  createCustomer (CustomerId "2") (Telephone "0992323421") (Some(Address "Peremohy ave 123"))

printfn "%A" customer2

type GenuineCustomer = GenuineCustomer of Customer

let validateCustomer customer =
  match customer.PrimaryContactDetails with
  | Email email when email.EndsWith "gmail.com" -> Some(GenuineCustomer customer)
  | Address _
  | Telephone _ -> Some(GenuineCustomer customer)
  | Email _ -> None

let sendWelcomeMessage (GenuineCustomer customer) =
  printfn "Hello, %A, and welcome to our site!" customer.CustomerId

let customer =
  createCustomer (CustomerId "1") (Email "alex@gmail.com") None

let welcomeMsg =
  customer
  |> validateCustomer
  |> Option.map sendWelcomeMessage

let insertContactUnsafe contactDetails =
  if contactDetails = (Email "nicki@myemail.com") then
    { CustomerId = CustomerId "ABC"
      PrimaryContactDetails = contactDetails
      SecondaryContactDetails = None }
  else failwith "Unable to insert  - customer already exists."
  
type Result<'a> =
  | Success of 'a
  | Failure of string

let insertContact contactDetails =
  if contactDetails = (Email "nicki@myemail.com") then
    Success { CustomerId = CustomerId "ABC"
              PrimaryContactDetails = contactDetails
              SecondaryContactDetails = None }
  else Failure "Unable to insert  - customer already exists."

match insertContact (Email "nicki2222@myemail.com") with
| Success customerId -> printfn "Saved with %A" customerId
| Failure error -> printfn "Unable to save: %s" error