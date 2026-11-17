import { useState } from 'react';
import Beslissingen from './beslissingen.jsx';
import ExtraBeslissingen from './extra_beslissingen.jsx';

export default function App() {
    const [huidigePagina, setHuidigePagina] = useState("beslissingen");

    return (
        <div>
            {huidigePagina === "beslissingen" ? (
                <Beslissingen onGaNaarExtra={() => setHuidigePagina("extra")} />
            ) : (
                <ExtraBeslissingen onGaNaarStandaard={() => setHuidigePagina("beslissingen")} />
            )}
        </div>
    );
}