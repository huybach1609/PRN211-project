import React, { useState } from 'react';
import { Button } from '@heroui/button';
import { Input } from '@heroui/input';
import { form, select, toast } from '@heroui/theme';
import { em } from 'framer-motion/client';
import axios from 'axios';
import { API_URL } from '../../constrains';
import { addToast } from '@heroui/react';
const SignupForm = ({ setIsLogin }) => {
    const [password, setPassword] = React.useState("");
    const [submitted, setSubmitted] = React.useState(null);
    const [errors, setErrors] = React.useState({});

    const SignInProcess = (username, password, email) => {
        //console.log(`${username} ${password} ${email}`);
        return axios
            .post(API_URL + '/auth/signup',
                { username, password, email },
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });

    }

    const SignUpPerform = (e) => {
        e.preventDefault();
        var username = e.target.username.value;
        var password = e.target.password.value;
        var email = e.target.email.value;

        //console.log(`${username} ${password} ${email}`);

        SignInProcess(username, password, email)
            .then((response) => {
                //console.log(response);

                addToast({
                    color: "success",
                    title: "Succes",
                    description: response.data.message

                });
                setTimeout(() => {
                    setIsLogin(true);
                }, 1000)
            })
            .catch((error) => {
                //console.log(error);

                const data = error.response.data.message;
                var errorMessage = "";
                if (error.response.data?.message) {
                    errorMessage = data; // plain string error
                    //console.log(error.response.data.message);

                } else if (error.response.data.errors) {
                    if (error.response.data.errors.Username?.[0]) {
                        errorMessage += error.response.data.errors.Username?.[0];
                    } else if (error.response.data.errors.Password?.[0]) {
                        errorMessage += error.response.data.errors.Password?.[0];
                    } else if (error.response.data.errors.Email?.[0]) {
                        errorMessage += error.response.data.errors.Email?.[0];
                    }
                    //console.log(error.response.data.errors);
                }

                addToast({
                    color: "danger",
                    title: "Fail",
                    description: errorMessage
                });
            })


    }
    return (
        <div className='w-auto'>
            <h2 className="text-2xl  font-bold mb-6 text-blue">Sign In</h2>
            <form onSubmit={SignUpPerform} method="post"
                className="w-[50vh] justify-center items-center space-y-4"
                onReset={() => setSubmitted(null)}
            >
                <div className="flex flex-col gap-4 max-w-md">
                    <Input
                        isRequired
                        errorMessage={({ validationDetails }) => {
                            if (validationDetails.valueMissing) {
                                return "Please enter your name";
                            }

                            return errors.name;
                        }}
                        label="Name"
                        labelPlacement="outside"
                        name="username"
                        placeholder="Enter your name"
                    />

                    <Input
                        isRequired
                        errorMessage={({ validationDetails }) => {
                            if (validationDetails.valueMissing) {
                                return "Please enter your email";
                            }
                            if (validationDetails.typeMismatch) {
                                return "Please enter a valid email address";
                            }
                        }}
                        label="Email"
                        labelPlacement="outside"
                        name="email"
                        placeholder="Enter your email"
                        type="email"
                    />

                    <Input
                        isRequired
                        label="Password"
                        labelPlacement="outside"
                        name="password"
                        placeholder="Enter your password"
                        type="password"
                        value={password}
                        onValueChange={setPassword}
                    />


                    <div className="flex gap-4">
                        <Button className="w-full text-white bg-blue" type="submit">
                            Sign Up
                        </Button>
                        <Button type="reset" variant="bordered">
                            Reset
                        </Button>
                    </div>
                </div>
            </form>
        </div>
    );
}
export default SignupForm;