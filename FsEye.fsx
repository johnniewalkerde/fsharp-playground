//reference FsEye.dll (e.g. "Send to F# Interactive" on the project reference availabe in some IDE versions)
#r @"C:\Users\johnn\Documents\Visual Studio 2015\Projects\fsharp-playground\packages\FsEye.2.1.0\lib\net40\FsEye.dll"

//Execute the following two lines of code to bring the eye singleton into scope and bind it to FSI
open Swensen.FsEye.Fsi //bring the eye singleton into scope
fsi.AddPrintTransformer eye.Listener //attached the listener

let processMessage message =
    printfn "Message: %A" message

let messageActor = MailboxProcessor.Start(fun inbox -> 
    let rec messageLoop() = async { 
        while true do
            let! message = inbox.Receive()
            processMessage message
            return! messageLoop()
    }
    messageLoop()
)

[1 .. 100] |> List.iter (fun x -> messageActor.Post ("Hallo " + x.ToString()))