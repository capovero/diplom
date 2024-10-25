import React, { useState } from "react";
import SignIn from "./SignIn";
import SignUp from "./SignUp";

const Registration = () => {
    const [inSignIn, setIsSignIn] = useState(true);
    return (
        <div>
            {inSignIn ? (
                <SignIn switchToSignUp={() => setIsSignIn(false)} />

            ) : (
                <SignUp switchToSignIn={() => setIsSignIn(true)} />

            )}
        </div>
    )
}
export default Registration;