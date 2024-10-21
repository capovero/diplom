import React from 'react';
import {Formik, Field, Form} from "formik";

const ModalWindow = props => {
    return (
        <div>
            <h1>Sign Up</h1>
            <Formik
            initialValues={{username: '', email: '',  password: ''}}
            onSubmit={(values, actions) => {
                console.log(values);
                actions.setSubmitting(false);
            }}>
                <Form>
                    <Field type="text" name="username" placeholder="Username" />
                    <Field type="email" name="email" placeholder="Email" />
                    <Field type="password" name="password" placeholder="Password" />
                    <button type="submit">Sign up</button>

                    {/*replace it with a login button*/}
                    <button type="button">Login</button>
                </Form>
            </Formik>
        </div>
    )
}
export default ModalWindow;