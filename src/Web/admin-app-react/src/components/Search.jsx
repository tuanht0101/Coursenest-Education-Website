import "font-awesome/css/font-awesome.min.css";

function Search({ setKeyWord }) {
    return (
        <div className="input-group">
            <input
                className="form-control border-end-0 border rounded-pill"
                type="search"
                placeholder="search"
                id="example-search-input"
                onChange={(e) => setKeyWord(e.target.value)}
            />
            <span className="input-group-append">
                <button
                    className="btn btn-outline-secondary border rounded-pill ms-n5"
                >
                    <i className="fa fa-search"></i>
                </button>
            </span>
        </div>
    );
}
export default Search;
